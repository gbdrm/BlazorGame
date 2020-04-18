using System;

namespace BlazorGame.Data
{
    public class QuizItem
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
    }
}
