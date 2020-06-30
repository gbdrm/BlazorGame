using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorGame.Data;
using BlazorGame.Data.Models;
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
        public async Task<long> MarkAsDoneAsync(Guid userId, Guid quizItemId)
        {
            var done = await _db.Completed.FirstOrDefaultAsync(c => c.UserId == userId && c.ItemId == quizItemId);
            var user = await _db.UserStates.FindAsync(userId);
            if (done == null)
            {
                var item = await _db.QuizItems.FindAsync(quizItemId);
                _db.Completed.Add(new Completed { ItemId = quizItemId, UserId = userId });
                user.Experience += item.ExperiencePoints;
                user.Level = (int)Math.Log(user.Experience, 2);
                await _db.SaveChangesAsync();
            }

            return user.Experience;
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
                    Experience = 0,
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
                .OrderByDescending(u => u.Experience)
                .Take(10)
                .ToDictionary(u => u.Name, u => u.Level);

            return result;
        }

        [HttpPost]
        [Route("SetName")]
        public async Task<bool> SetNameAsync(UserState input)
        {
            if (string.IsNullOrWhiteSpace(input.Name) || input.Name.Length < 3) return false;

            var isUnique = await _db.UserStates.FirstOrDefaultAsync(u => u.Name == input.Name);
            if (isUnique != null) return false;

            var user = await _db.UserStates.FindAsync(input.UserId);
            user.Name = input.Name;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
