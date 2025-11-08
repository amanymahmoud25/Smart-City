using System.ComponentModel.DataAnnotations;

namespace Smart_City.Dtos
{
    public class SuggestionUpdateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
