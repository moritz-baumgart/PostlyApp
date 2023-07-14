using PostlyApp.Enums;
using PostlyApp.Models.DTOs;
using PostlyApp.Models.Requests;
using PostlyApp.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PostlyApp.Services.Impl
{
    internal class ContentService : IContentService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly IJwtService _jwt;

        public ContentService()
        {
#if DEBUG && ANDROID
            var handlerService = new HttpsClientHandlerService();
            _client = new HttpClient(handlerService.GetPlatformMessageHandler());
#else
            _client = new HttpClient();

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
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


        public async Task<int?> AddComment(int postId, string commentContent)
        {
            var uriBuilder = new UriBuilder(Constants.API_BASE + "/comment");

            var json = JsonSerializer.Serialize(new CommentCreateRequest
            {
                PostId = postId,
                CommentContent = commentContent,
            }, _serializerOptions);


            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var res = await _client.PostAsync(uriBuilder.ToString(), content);
                if (res.IsSuccessStatusCode)
                {
                    var newCommentCount = await ApiUtilities.DeserializeJsonResponse<int>(res);
                    return newCommentCount;
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

        public async Task<List<CommentDTO>?> GetComments(long postId)
        {
            var uriBuilder = new UriBuilder(Constants.API_BASE + $"/post/{postId}/comments");

            try
            {
                var res = await _client.GetAsync(uriBuilder.ToString());
                if (res.IsSuccessStatusCode)
                {
                    return await ApiUtilities.DeserializeJsonResponse<List<CommentDTO>?>(res);
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
            try
            {
                var param = new Dictionary<string, string>();
                if (paginationStart != null)
                {
                    param.Add("paginationStart", ((DateTimeOffset)paginationStart).ToString("o"));
                }

                var uri = ApiUtilities.BuildUri("/feed/public", param);
                var res = await _client.GetAsync(uri.ToString());

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


        public async Task<List<PostDTO>?> GetPrivateFeed(DateTimeOffset? paginationStart)
        {
            try
            {
                var param = new Dictionary<string, string>();
                if (paginationStart != null)
                {
                    param.Add("paginationStart", ((DateTimeOffset)paginationStart).ToString("o"));
                }

                var uri = ApiUtilities.BuildUri("/feed/private", param);
                var res = await _client.GetAsync(uri.ToString());

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

        public async Task<List<PostDTO>?> GetProfileFeed(string? username, DateTimeOffset? paginationStart)
        {
            try
            {
                var param = new Dictionary<string, string>();
                if (paginationStart != null)
                {
                    param.Add("paginationStart", ((DateTimeOffset)paginationStart).ToString("o"));
                }

                var uri = ApiUtilities.BuildUri($"/feed/profile/{ username ?? "me" }", param);
                var res = await _client.GetAsync(uri.ToString());

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
