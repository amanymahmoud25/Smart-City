using Smart_City.Models;
using System.ComponentModel.DataAnnotations;

namespace Smart_City.Dtos
{
    public class UtilityIssueCreateDto
    {
        [Required]
        public UtilityIssueType Type { get; set; } 
        [Required]
        public string Description { get; set; }
    }

    public class UtilityIssueDto
    {
        public int Id { get; set; }
        public UtilityIssueType Type { get; set; }
        public string Description { get; set; }
        public DateTime ReportDate { get; set; }
        public string Status { get; set; }
        public UserDto Citizen { get; set; }
    }

    public class UtilityIssueUpdateDto
    {
        [Required]
        public int Id { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}
