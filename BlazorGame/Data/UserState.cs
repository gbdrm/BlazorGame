using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGame.Data
{
    public class UserState
    {
        public int CurrentScore { get; set; }

        public Guid UserId { get; set; }
    }
}
