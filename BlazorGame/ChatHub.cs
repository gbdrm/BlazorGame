using BlazorGame.Data.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGame.Data
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _db;

        public ChatHub(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task SendMessage(string text, Guid userId)
        {
            var msg = new Message
            {
                Created = DateTime.UtcNow,
                Text = text
            };

            if (userId != Guid.Empty)
            {
                msg.UserId = userId;
            }

            _db.Messages.Add(msg);
            _db.SaveChanges();

            msg.User = _db.UserStates.Find(userId);
            await Clients.All.SendAsync("NewMessage", msg);
        }

        public List<Message> GetMessages()
        {
            return _db.Messages.Include(m => m.User).ToList();
        }
    }
}
