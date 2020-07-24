using System;
using System.Collections.Generic;

namespace Polls.API.Models.Output
{
    public class PollVoteInputDto
    {
        public ICollection<Guid> Votes { get; set; }
    }
}