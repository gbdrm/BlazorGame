using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorGame.Data.Models
{
    public class UserState
    {
        [Key]
        [Required]
        public Guid UserId { get; set; }

        public bool CanCreate { get; set; }

        public string Name { get; set; }


        public int Level { get; set; }
        public long Experience { get; set; }
    }
}
