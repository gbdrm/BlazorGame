using BlazorGame.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorGame.Client.Service
{
    public class QuizService
    {
        private HttpClient httpClient;

        public QuizService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<TRes> GetFromController<TRes>(string method)
        {
            var response = await httpClient.GetAsync("/api/quizcontroller/" + method);
            var strContent = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            var result = JsonSerializer.Deserialize<TRes>(strContent, options);
            //result.RawResponse = strContent;

            return result;
        }

        public async Task<TRes> PostToController<TRes>(string method, object parameters)
        {
            var json = JsonSerializer.Serialize(parameters);

            var response = await httpClient.PostAsync("/api/quizcontroller/" + method, new StringContent(json));
            var strContent = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            var result = JsonSerializer.Deserialize<TRes>(strContent, options);
            //result.RawResponse = strContent;

            return result;
        }

        public async Task<List<QuizItem>> GetQuizAsync(Guid userId)
        {
            var result = await PostToController<List<QuizItem>>("GetQuizAsync", new { userId });
            return result;
        }

        public async Task<int?> MarkAsDoneAsync(Guid userId, Guid quizItemId)
        {
            var result = await PostToController<int?>("GetQuizAsync", new { userId, quizItemId });
            return result;
        }

        public async Task<UserState> GetStateAsync(Guid userId)
        {
            var result = await PostToController<UserState>("GetStateAsync", new { userId });
            return result;
        }

        public async Task CreateQuizItemAsync(Guid userId, QuizItem quizItem)
        {
            var result = await PostToController<Task>("CreateQuizItemAsync", new { userId, quizItem });
        }

        public async Task<Dictionary<string, int>> GetLeaders()
        {
            var result = await GetFromController<Dictionary<string, int>>("GetStateAsync");
            return result;
        }

        public async Task<bool> SetNameAsync(Guid userId, string name)
        {
            var result = await PostToController<bool>("SetNameAsync", new { userId, name });
            return result;
        }
    }
}
