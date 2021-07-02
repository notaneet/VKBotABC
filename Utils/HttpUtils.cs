using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VKBotABC.Utils
{
    public static class HttpUtils
    {
        private static HttpClient _httpClient = new HttpClient();


        public static async Task<JToken> GetVk(string method, Assoc<string, object> parameters)
        {
            return await Get(Method(method, parameters));
        }

        public static async Task<JToken> Get(string method, Assoc<string, object> parameters)
        {
            return await Get(Build(method, parameters));
        }
        
        public static async Task<JToken> Get(Uri uri)
        {
            Console.Out.WriteLine(uri);
            var response = await _httpClient.GetStringAsync(uri);
            Console.Out.WriteLine(response);

            return JsonConvert.DeserializeObject<JToken>(response);
        }

        public static Uri Method(string method, Assoc<string, object> parameters)
        {
            parameters.Add("v", BotAbc.ApiVersion);
            return Build(BotAbc.ApiEndpoint + method, parameters);
        }

        public static Uri Build(string baseUrl, Assoc<string, object> parameters)
        {
            var stringifiedParameters = string.Join("&", parameters.Select(i =>
                $"{Uri.EscapeDataString(i.Key)}={Uri.EscapeDataString(i.Value.ToString())}"));
            var requestUrl = string.Concat($"{baseUrl}?", stringifiedParameters);
            return new Uri(requestUrl);
        }
    }
}