using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Polls.API.Entities
{
    public class Poll
    {
        [Key] public Guid Id { get; set; }

        public string Slug { get; set; }
        
        [Required]
        public string Question { get; set; }
        
        public bool MultiSelect { get; set; }
        
        public int TotalVotes { get; set; }
        
        public virtual ICollection<Vote> Votes { get; set; }
        
        public virtual ICollection<Option> Options { get; set; }
    }
}