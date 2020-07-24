using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Polls.API.Models.Input
{
    public class PollInputDto
    {
        public string Slug { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Question { get; set; }

        public bool MultiSelect { get; set; }

        public ICollection<OptionInputDto> Options { get; set; }
    }
}