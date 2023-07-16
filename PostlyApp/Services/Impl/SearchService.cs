using PostlyApp.Models.DTOs;
using PostlyApp.Utilities;
using System.Text.Json;
using System.Web;

namespace PostlyApp.Services.Impl
{
    class SearchService : ISearchService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _serializerOptions;

        public SearchService()
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
        }

        /// <summary>
        /// Searches for users and returns the search result.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="UserDTO"/> if successful, null otherwise.</returns>
        public async Task<IEnumerable<UserDTO>?> SearchUsers(string username)
        {
            try
            {
                var uri = ApiUtilities.BuildUri("/search", new()
                {
                    { "username", username }
                });
                var res = await _client.GetAsync(uri);

                if (res.IsSuccessStatusCode)
                {
                    return await ApiUtilities.DeserializeJsonResponse<IEnumerable<UserDTO>>(res);
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
