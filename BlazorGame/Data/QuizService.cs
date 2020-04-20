using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGame.Data
{
    public class QuizService
    {
        private static readonly List<QuizItem> QuizItems;
        private static readonly Dictionary<Guid, HashSet<Guid>> Completed = new Dictionary<Guid, HashSet<Guid>>();

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

        public async Task<List<QuizItem>> GetQuizesAsync(Guid userId)
        {
            var completed = Completed.ContainsKey(userId) ? Completed[userId] : null;
            List<QuizItem> result;
            if (completed == null || completed.Count == 0) result = QuizItems;
            else result = QuizItems.Where(i => !completed.Contains(i.Id)).ToList();

            return result;
        }

        public async Task MarkAsDoneAsync(Guid userId, Guid quizItemId)
        {
            if (!Completed.ContainsKey(userId))
            {
                Completed.Add(userId, new HashSet<Guid>());
            }

            Completed[userId].Add(quizItemId);
        }
    }
}
