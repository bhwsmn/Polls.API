using System.ComponentModel.DataAnnotations;

namespace Polls.API.Models.Input
{
    public class OptionInputDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Text { get; set; }
    }
}