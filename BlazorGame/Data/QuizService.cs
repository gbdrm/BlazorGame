using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGame.Data
{
    public class QuizService
    {
        private readonly ApplicationDbContext _db;

        public QuizService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<QuizItem>> GetQuizesAsync(Guid userId)
        {
            var result = await _db.QuizItems
                .Where(q => !q.Completed.Any(c => c.UserId == userId))
                .ToListAsync();

            return result;
        }

        public async Task MarkAsDoneAsync(Guid userId, Guid quizItemId)
        {
            var done = await _db.Completed.FirstOrDefaultAsync(c => c.UserId == userId && c.ItemId == quizItemId);
            if(done == null)
            {
                _db.Completed.Add(new Models.Completed { ItemId = quizItemId, UserId = userId });
                await _db.SaveChangesAsync();
            }
        }

        public async Task<UserState> GetStateAsync(Guid userId)
        {
            var user = await _db.UserStates.FindAsync(userId);
            if(user == null)
            {
                user = new UserState
                {
                    CurrentScore = 0,
                    UserId = userId,
                    CanCreate = true
                };
                
                await _db.UserStates.AddAsync(user);
                await _db.SaveChangesAsync();
            }

            return user;
        }

        public async Task CreateQuizItemAsync(Guid userId, QuizItem quizItem)
        {
            var user = await _db.UserStates.FindAsync(userId);
            if (user.CanCreate)
            {
                await _db.QuizItems.AddAsync(quizItem);
                await _db.SaveChangesAsync();

                user.CanCreate = false;
                await _db.SaveChangesAsync();
            }
        }
    }
}
