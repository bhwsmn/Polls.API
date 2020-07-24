using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polls.API.Entities
{
    public class Vote
    {
        [Key] 
        public Guid Id { get; set; }

        public string IpAddress { get; set; }

        [ForeignKey("PollId")] 
        public virtual Poll Poll { get; set; }

        public virtual Guid PollId { get; set; }
    }
}