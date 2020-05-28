using BlazorGame.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorGame.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserState> UserStates { get; set; }
        public DbSet<QuizItem> QuizItems { get; set; }
        public DbSet<Completed> Completed { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
