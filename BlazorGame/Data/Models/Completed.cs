using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGame.Data.Models
{
    public class Completed
    {
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public UserState User { get; set; }

        [ForeignKey("Item")]
        public Guid ItemId { get; set; }
        public QuizItem Item { get; set; }
    }
}
