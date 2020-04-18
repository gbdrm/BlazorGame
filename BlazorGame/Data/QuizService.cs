using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorGame.Data
{
    public class QuizService
    {
        private static readonly List<QuizItem> QuizItems;

        static QuizService()
        {
            QuizItems = new List<QuizItem> {
                new QuizItem
                {
                    Question = "4 + 7 = ?",
                    Answer = "11",
                    Score = 1
                },
                new QuizItem
                {
                    Question = "Where is the code of this application hosted?",
                    Answer = "Github",
                    Score = 5
                }
            };
        }

        public Task<List<QuizItem>> GetQuizesAsync()
        {
            return Task.FromResult(QuizItems);
        }
    }
}
