using PostlyApp.Enums;
using PostlyApp.Models.DTOs;
using PostlyApp.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace PostlyApp.Services.Impl
{
    internal class ContentService : IContentService
    {
        private readonly HttpClient _client;
        private readonly IJwtService _jwt;

        public ContentService()
        {
#if DEBUG && ANDROID
            var handlerService = new HttpsClientHandlerService();
            _client = new HttpClient(handlerService.GetPlatformMessageHandler());
#else
            _client = new HttpClient();
#endif
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

        public event Action<int> OnNewPostCreated;

        public async Task<int?> AddPost(string content)
        {
            var uriBuilder = new UriBuilder(Constants.API_BASE + "/post");
            var body = ApiUtilities.SerializeJsonBody(content);

            try
            {
                var res = await _client.PostAsync(uriBuilder.ToString(), body);
                if (res.IsSuccessStatusCode)
                {
                    var newPostId = await ApiUtilities.DeserializeJsonResponse<int>(res);
                    OnNewPostCreated.Invoke(newPostId);
                    return newPostId;
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

        public async Task<PostDTO?> GetPost(long postId)
        {
            var uriBuilder = new UriBuilder(Constants.API_BASE + $"/post/{postId}");

            try
            {
                var res = await _client.GetAsync(uriBuilder.ToString());
                if (res.IsSuccessStatusCode)
                {
                    return await ApiUtilities.DeserializeJsonResponse<PostDTO>(res);
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

        public async Task<List<PostDTO>?> GetPublicFeed(DateTimeOffset? paginationStart)
        {
            var uriBuilder = new UriBuilder(Constants.API_BASE + "/feed/public");

            try
            {
                var param = new Dictionary<string, string>();
                if (paginationStart != null)
                {
                    param.Add("paginationStart", ((DateTimeOffset)paginationStart).ToString("o"));
                }

                var uri = ApiUtilities.BuildUri("/feed/public", param);
                var res = await _client.GetAsync(uriBuilder.ToString());

                if (res.IsSuccessStatusCode)
                {
                    return await ApiUtilities.DeserializeJsonResponse<List<PostDTO>>(res);
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

        public async Task<VoteUpdateViewModel?> SetVote(int postId, VoteType voteType)
        {
            var uriBuilder = new UriBuilder(Constants.API_BASE + $"/post/{postId}/vote");
            var body = ApiUtilities.SerializeJsonBody(voteType);

            try
            {
                var res = await _client.PostAsync(uriBuilder.ToString(), body);
                if (res.IsSuccessStatusCode)
                {
                    return await ApiUtilities.DeserializeJsonResponse<VoteUpdateViewModel?>(res);
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

        public async Task<VoteUpdateViewModel?> RemoveVote(int postId)
        {
            var uriBuilder = new UriBuilder(Constants.API_BASE + $"/post/{postId}/vote");

            try
            {
                var res = await _client.DeleteAsync(uriBuilder.ToString());
                if (res.IsSuccessStatusCode)
                {
                    return await ApiUtilities.DeserializeJsonResponse<VoteUpdateViewModel?>(res);
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
