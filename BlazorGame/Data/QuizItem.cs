using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorGame.Data
{
    public class QuizItem
    {
        public Guid Id { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}
