using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGame.Data.Models
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Text { get; set; }

        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public UserState User { get; set; }
    }
}
