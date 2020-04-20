using System;

namespace BlazorGame.Data
{
    public class UserState
    {
        public int CurrentScore { get; set; }

        public Guid UserId { get; set; }

        public bool CanCreate { get; set; }
    }
}
