using System.ComponentModel.DataAnnotations;

namespace Smart_City.Dtos
{
    public class SuggestionStatusUpdateDto
    {
        [Required]
        public string Status { get; set; }
    }
}
