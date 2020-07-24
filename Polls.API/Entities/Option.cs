using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polls.API.Entities
{
    public class Option
    {
        [Key] 
        public Guid Id { get; set; }

        [Required]
        public string Text { get; set; }
        
        public int Votes { get; set; }

        [ForeignKey("PollId")] 
        public virtual Poll Poll { get; set; }

        public virtual Guid PollId { get; set; }
    }
}