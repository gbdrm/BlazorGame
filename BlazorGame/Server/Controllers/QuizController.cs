using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorGame.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorGame.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public QuizController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("GetQuiz")]
        public async Task<List<QuizItem>> GetQuizAsync(Guid userId)
        {
            var result = await _db.QuizItems
                .Where(q => !q.Completed.Any(c => c.UserId == userId))
                .Take(3)
                .ToListAsync();

            return result;
        }

        [HttpGet]
        [Route("MarkAsDone")]
        public async Task<int> MarkAsDoneAsync(Guid userId, Guid quizItemId)
        {
            var done = await _db.Completed.FirstOrDefaultAsync(c => c.UserId == userId && c.ItemId == quizItemId);
            var user = await _db.UserStates.FindAsync(userId);
            if (done == null)
            {
                _db.Completed.Add(new Data.Models.Completed { ItemId = quizItemId, UserId = userId });
                user.CurrentScore++;
                await _db.SaveChangesAsync();
            }

            return user.CurrentScore;
        }

        [HttpGet]
        [Route("GetState")]
        public async Task<UserState> GetStateAsync(Guid userId)
        {
            var user = await _db.UserStates.FindAsync(userId);
            if (user == null)
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

        public class Question
        {
            public Guid UserId { get; set; }
            public QuizItem QuizItem { get; set; }
        }

        [HttpPost]
        [Route("CreateQuizItem")]
        public async Task CreateQuizItemAsync(Question question)
        {
            var user = await _db.UserStates.FindAsync(question.UserId);
            if (user.CanCreate)
            {
                await _db.QuizItems.AddAsync(question.QuizItem);
                await _db.SaveChangesAsync();

                user.CanCreate = false;
                await _db.SaveChangesAsync();
            }
        }

        [HttpGet]
        [Route("GetLeaders")]
        public Dictionary<string, int> GetLeaders()
        {
            var result = _db.UserStates
                .Where(u => u.Name != null)
                .OrderByDescending(u => u.CurrentScore)
                .Take(10)
                .ToDictionary(u => u.Name, u => u.CurrentScore);

            return result;
        }

        [Route("SetName")]
        public async Task<bool> SetNameAsync(Guid userId, string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 3) return false;

            var isUnique = await _db.UserStates.FirstOrDefaultAsync(u => u.Name == name);
            if (isUnique != null) return false;

            var user = await _db.UserStates.FindAsync(userId);
            user.Name = name;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
