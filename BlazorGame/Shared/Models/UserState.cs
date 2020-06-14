using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorGame.Data
{
    public class UserState
    {
        public int CurrentScore { get; set; }

        [Key]
        [Required]
        public Guid UserId { get; set; }

        public bool CanCreate { get; set; }

        public string Name { get; set; }
    }
}
