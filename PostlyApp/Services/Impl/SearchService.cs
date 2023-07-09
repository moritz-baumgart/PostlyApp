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
                    return await ApiUtilities.DecodeJsonResponse<IEnumerable<UserDTO>>(res);
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
