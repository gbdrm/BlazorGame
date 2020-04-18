using System;
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
                    Id = Guid.NewGuid(),
                    Question = "4 + 7 = ?",
                    Answer = "11",
                    Score = 1
                },
                new QuizItem
                {
                    Id = Guid.NewGuid(),
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
