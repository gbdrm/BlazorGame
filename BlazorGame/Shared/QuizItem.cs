using BlazorGame.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorGame.Data.Models
{
    public class QuizItem
    {
        public Guid Id { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }

        public int ExperiencePoints { get; set; } = 1;

        public virtual ICollection<Completed> Completed { get; set; }
    }
}
