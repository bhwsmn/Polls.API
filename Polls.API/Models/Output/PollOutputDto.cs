using System.Collections.Generic;

namespace Polls.API.Models.Output
{
    public class PollOutputDto
    {
        public string Slug { get; set; }
        public string Question { get; set; }
        public bool MultiSelect { get; set; }
        public int TotalVotes { get; set; }
        public ICollection<OptionOutputDto> Options { get; set; }
    }
}