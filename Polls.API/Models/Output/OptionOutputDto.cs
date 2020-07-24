using System;

namespace Polls.API.Models.Output
{
    public class OptionOutputDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int Votes { get; set; }
    }
}