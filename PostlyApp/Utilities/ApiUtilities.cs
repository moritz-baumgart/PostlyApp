using System.Text;
using System.Text.Json;
using System.Web;

namespace PostlyApp.Utilities
{
    class ApiUtilities
    {
        /// <summary>
        /// Default options used for json serialization and deserialization.
        /// </summary>
        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };


        /// <summary>
        /// Builds an uri with query paramters.
        /// </summary>
        /// <param name="path">The path of the uri.</param>
        /// <param name="parameters">A <see cref="Dictionary{TKey, TValue}"/> with query parameter key value pairs.</param>
        /// <returns>The uri as a string.</returns>
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

        /// <summary>
        /// Deserializes a http response into given type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize into.</typeparam>
        /// <param name="response">The http response to read the content from.</param>
        /// <returns>The deserialized object of type T.</returns>
        public static async Task<T?> DeserializeJsonResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _serializerOptions);
        }

        /// <summary>
        /// Serializes a given object to be used in a request.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="body">The object to serialize.</param>
        /// <returns>A <see cref="StringContent"/> containing the serialization result.</returns>
        public static StringContent SerializeJsonBody<T>(T body)
        {
            string json = JsonSerializer.Serialize(body, _serializerOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
