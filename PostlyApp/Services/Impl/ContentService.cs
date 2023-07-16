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
            // If inside the emulator we have to initialize the http client to ignore invalid ssl certs.
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
        /// An event invoked when a new post was created. Can be used to update views when the user posts something.
        /// </summary>
        public event Action<int> OnNewPostCreated;

        /// <summary>
        /// Tries to create a new post.
        /// </summary>
        /// <param name="content">The text content of the post.</param>
        /// <returns>The new post's id if successful, null otherwise.</returns>
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

        /// <summary>
        /// Fetches a post.
        /// </summary>
        /// <param name="postId">The id of the post to fetch.</param>
        /// <returns>A <see cref="PostDTO"/> if successful, null otherwise.</returns>
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

        /// <summary>
        /// Adds a comment to a post.
        /// </summary>
        /// <param name="postId">The id of the post the comment should be added to.</param>
        /// <param name="commentContent">The text content of the comment.</param>
        /// <returns>The new number of comments on that post if the request was successful, null otherwise.</returns>
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

        /// <summary>
        /// Fetches the comments of a post.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns>A <see cref="List{T}"/> of <see cref="CommentDTO"/>s.</returns>
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

        /// <summary>
        /// Fetches the public feed.
        /// </summary>
        /// <param name="paginationStart">The start of the feed, all fetched post will be older than this.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="PostDTO"/> if successful, null otherwise.</returns>
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

        /// <summary>
        /// Fetches the private feed.
        /// </summary>
        /// <param name="paginationStart">The start of the feed, all fetched post will be older than this.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="PostDTO"/> if successful, null otherwise.</returns>
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

        /// <summary>
        /// Fetches the posts of a single user.
        /// </summary>
        /// <param name="username">The username of the user to fetch the posts for.</param>
        /// <param name="paginationStart">The start of the feed, all fetched post will be older than this.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="PostDTO"/> if successful, null otherwise.</returns>
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

        /// <summary>
        /// Submits a vote for a post.
        /// </summary>
        /// <param name="postId">The id of the post to apply the vote on.</param>
        /// <param name="voteType">The type of vote to submit.</param>
        /// <returns>A <see cref="VoteUpdateViewModel"/> with new posts data if successful, null otehrwise.</returns>
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

        /// <summary>
        /// Removes a vote from a post.
        /// </summary>
        /// <param name="postId">The id of the post to remove the vote from</param>
        /// <returns>A <see cref="VoteUpdateViewModel"/> with new posts data if successful, null otehrwise.</returns>
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
