using PostlyApp.Utilities;
using System.Text.Json;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Net.Http.Headers;
using System.Text;
using PostlyApp.Models.Requests;
using System.Net;
using PostlyApp.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace PostlyApp.Services.Impl
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly IJwtService _jwt;

        public AccountService()
        {
            // If inside the emulator we have to initialize the http client to ignore invalid ssl certs.
#if DEBUG && ANDROID
            var handlerService = new HttpsClientHandlerService();
            _client = new HttpClient(handlerService.GetPlatformMessageHandler());
#else
            _client = new HttpClient();
#endif
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            _jwt = DependencyService.Resolve<IJwtService>();
            _jwt.CurrentTokenChanged += OnTokenChange;
        }

        /// <summary>
        /// Is called by the <see cref="JwtService.CurrentTokenChanged"/> event and updates the http clients headers.
        /// </summary>
        /// <param name="token">The new token to be used from now on.</param>
        private void OnTokenChange(JwtSecurityToken? token)
        {
            if (token != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.RawData);
            }
            else
            {
                _client.DefaultRequestHeaders.Authorization = null;
            }
        }


        /// <summary>
        /// Retrieves the current login state.
        /// </summary>
        /// <returns>False if no jwt is set or the user is not authenticated, true otherwise.</returns>
        public async Task<bool> GetStatus()
        {
            var uri = new Uri(Constants.API_BASE + "/account/status");
            try
            {
                var response = await _client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<bool>(content, _serializerOptions);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return false;
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception)
            {
                await ShowConnectionError();
                return false;
            }
        }

        /// <summary>
        /// Fetches the profile details of a users.
        /// </summary>
        /// <param name="username">The username of the user to fetch the details for.</param>
        /// <returns>An <see cref="UserProfileViewModel"/> if the request was successful, null otherwise.</returns>
        public async Task<UserProfileViewModel?> GetUserProfile(string? username)
        {
            var uri = new Uri(Constants.API_BASE + $"/account/{username ?? "me"}/profile");

            try
            {
                var res = await _client.GetAsync(uri);

                if (!res.IsSuccessStatusCode)
                {
                    return null;
                }


                return await ApiUtilities.DeserializeJsonResponse<UserProfileViewModel>(res);

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches the users that follow the specified user.
        /// </summary>
        /// <param name="username">The username of the user to fetch the followers for.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="UserDTO"/> if the request was successful, null otherwise.</returns>
        public async Task<List<UserDTO>?> GetFollowers(string? username)
        {
            var uri = new Uri(Constants.API_BASE + $"/account/{username ?? "me"}/followers");

            try
            {
                var res = await _client.GetAsync(uri);

                if (!res.IsSuccessStatusCode)
                {
                    return null;
                }


                return await ApiUtilities.DeserializeJsonResponse<List<UserDTO>?>(res);

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches the users the specified user follows.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="UserDTO"/> if the request was successful, null otherwise.</returns>
        public async Task<List<UserDTO>?> GetFollowing(string? username)
        {
            var uri = new Uri(Constants.API_BASE + $"/account/{username ?? "me"}/following");

            try
            {
                var res = await _client.GetAsync(uri);

                if (!res.IsSuccessStatusCode)
                {
                    return null;
                }


                return await ApiUtilities.DeserializeJsonResponse<List<UserDTO>?>(res);

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Logs in a user with given credentials. Also saves jwt using <see cref="JwtService"/>.
        /// </summary>
        /// <returns>True if login was successful, false if username/password was wrong, null if something went wrong.</returns>
        public async Task<bool?> Login(string username, string password)
        {
            var uri = new Uri(Constants.API_BASE + "/account/login");

            var json = JsonSerializer.Serialize(new LoginOrRegisterRequest
            {
                Username = username,
                Password = password
            }, _serializerOptions);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    _jwt.SetCurrentTokenFromString(token);
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return false;
                }
                return null;
            }
            catch (Exception)
            {
                await AccountService.ShowConnectionError();
                return null;
            }

        }



        /// <summary>
        /// Logs the user out by deleting the JWT and navigating back to the login.
        /// </summary>
        public async void Logout()
        {
            _jwt.DeleteCurrentToken();
            await Shell.Current.GoToAsync("//Login");
        }

        private static async Task ShowConnectionError()
        {
            var toast = Toast.Make("Error connecting to server. Try again later!", ToastDuration.Long);
            await toast.Show();
        }

        /// <summary>
        /// Follows the given user.
        /// </summary>
        /// <returns>An updated <see cref="UserProfileViewModel"/> if the request was successful, null otherwise.</returns>

        public Task<UserProfileViewModel?> FollowUser(string username)
        {
            return ChangeFollow(username, true);
        }

        /// <summary>
        /// Unfollows the given user.
        /// </summary>
        /// <returns>An updated <see cref="UserProfileViewModel"/> if the request was successful, null otherwise.</returns>
        public Task<UserProfileViewModel?> UnfollowUser(string username)
        {
            return ChangeFollow(username, false);
        }

        /// <summary>
        /// Helper method used by <see cref="FollowUser(string)"/> and <see cref="UnfollowUser(string)"/>.
        /// </summary>
        /// <param name="username">The username to change the following state for.</param>
        /// <param name="isFollow">If true, follows the user, if false unfollows the user.</param>
        /// <returns>An updated <see cref="UserProfileViewModel"/> if the request was successful, null otherwise.</returns>
        private async Task<UserProfileViewModel?> ChangeFollow(string username, bool isFollow)
        {
            var uriBuilder = new UriBuilder(Constants.API_BASE + $"/account/me/following/{username}");

            try
            {
                HttpResponseMessage res;
                if (isFollow)
                {
                    res = await _client.PostAsync(uriBuilder.ToString(), null);
                }
                else
                {
                    res = await _client.DeleteAsync(uriBuilder.ToString());
                }

                if (res.IsSuccessStatusCode)
                {
                    return await ApiUtilities.DeserializeJsonResponse<UserProfileViewModel>(res);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }


        }

        /// <summary>
        /// Tries to registers a user with the given credentials.
        /// </summary>
        /// <returns>Returns true if successful, false if the username is already in use, null if something went wrong.</returns>
        public async Task<bool?> Register(string username, string password)
        {
            var uriBuilder = new UriBuilder(Constants.API_BASE + "/account/register");
            var body = ApiUtilities.SerializeJsonBody(new LoginOrRegisterRequest
            {
                Username = username,
                Password = password
            });

            try
            {
                var res = await _client.PostAsync(uriBuilder.ToString(), body);
                if (res.IsSuccessStatusCode)
                {
                    return true;
                }
                else if (res.StatusCode == HttpStatusCode.Conflict)
                {
                    return false;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
