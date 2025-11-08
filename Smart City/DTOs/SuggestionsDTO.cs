using System.ComponentModel.DataAnnotations;

namespace Smart_City.Dtos
{
    public class SuggestionCreateDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }

    public class SuggestionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Status { get; set; }
        public UserDto Citizen { get; set; }
    }
}
