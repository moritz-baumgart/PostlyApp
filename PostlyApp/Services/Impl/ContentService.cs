using PostlyApp.Models.DTOs;
using PostlyApp.Utilities;
using System.Text.Json;
using System.Web;

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
#endif
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            _jwt = DependencyService.Resolve<IJwtService>();
        }



        public async Task<List<PostDTO>> GetPublicFeed(DateTimeOffset? paginationStart)
        {
            var uriBuilder = new UriBuilder(Constants.API_BASE + "/feed/public");

            try
            {
                if (paginationStart != null)
                {
                    string paginationStartJson = JsonSerializer.Serialize(paginationStart, _serializerOptions);
                    var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                    query["paginationStart"] = paginationStartJson;
                    uriBuilder.Query = query.ToString();
                }

                var res = await _client.GetAsync(uriBuilder.ToString());

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<PostDTO>>(content, _serializerOptions);
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
