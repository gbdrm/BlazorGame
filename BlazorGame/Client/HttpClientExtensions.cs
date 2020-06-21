using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorGame.Client
{
    [Obsolete]
    public static class HttpClientExtensions
    {
        private static JsonSerializerOptions defaultSerialization;
        static HttpClientExtensions()
        {
            defaultSerialization = new JsonSerializerOptions();
            defaultSerialization.PropertyNameCaseInsensitive = true;
        }

        public static async Task<TRes> GetFromAction<TRes>(this HttpClient httpClient, string controller, string action, string query = null)
        {
            var response = await httpClient.GetAsync($"{controller.ToLower()}/{action.ToLower()}?{query}");
            var strContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TRes>(strContent, defaultSerialization);
            return result;
        }

        public static async Task<TRes> PostToAction<TRes>(this HttpClient httpClient, string controller, string action, object parameters)
        {
            //string json = await Task.Run(() => JsonConvert.SerializeObject(createLoginPayload(usernameTextBox.Text, password)));
            //var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            //using (var httpClient = new HttpClient())
            //{
            //    // Error here
            //    var httpResponse = await httpClient.PostAsync("URL HERE", httpContent);
            //    if (httpResponse.Content != null)
            //    {
            //        // Error Here
            //        var responseContent = await httpResponse.Content.ReadAsStringAsync();
            //    }
            //}

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, httpClient.BaseAddress + $"{controller.ToLower()}/{action.ToLower()}");
            request.Content = new StringContent(JsonSerializer.Serialize(parameters),
                                                Encoding.UTF8,
                                                "application/json");

            var response = await httpClient.SendAsync(request);
            var strContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TRes>(strContent, defaultSerialization);
            return result;
        }
    }
}
