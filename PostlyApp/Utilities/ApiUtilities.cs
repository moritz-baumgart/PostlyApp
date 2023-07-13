using System.Text;
using System.Text.Json;
using System.Web;

namespace PostlyApp.Utilities
{
    class ApiUtilities
    {
        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };


        public static string BuildUri(string path, Dictionary<string, string> parameters)
        {
            var uriBuilder = new UriBuilder(Constants.API_BASE + path);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var param in parameters)
            {
                query[param.Key] = param.Value;
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }

        public static async Task<T?> DeserializeJsonResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _serializerOptions);
        }

        public static StringContent SerializeJsonBody<T>(T body)
        {
            string json = JsonSerializer.Serialize(body, _serializerOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
