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
        /// Logs in a user with given credentials. Also saves jwt.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
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
        /// Logs the user out by deleting the JWT and navigating back to the login
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
    }
}
