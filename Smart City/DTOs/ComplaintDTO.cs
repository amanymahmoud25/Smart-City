using Smart_City.Dtos;
using Smart_City.Models;
using System.ComponentModel.DataAnnotations;

namespace Smart_City.Dtos
{
    public class ComplaintCreateDto
    {
        [Required, MinLength(5), MaxLength(120)]
        public string Title { get; set; }

        [Required, MaxLength(2000)]
        public string Description { get; set; }

        public string Location { get; set; }
        public string ImageUrl { get; set; }
    }

    public class ComplaintUpdateDto
    {
        [MinLength(5), MaxLength(120)]
        public string? Title { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        public string? Location { get; set; }
        public string? ImageUrl { get; set; }
    }
}

public class ComplaintDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateSubmitted { get; set; }
    public ComplaintStatus Status { get; set; }
    public UserBriefDto Citizen { get; set; }
    public string ImageUrl { get; set; }
    public string Location { get; set; }
    public string AdminNote { get; set; }
    public int CitizenId { get; set; }
    public int? AdminId { get; set; }
}

public class AdminNoteDto
{
    [Required, MinLength(2), MaxLength(1000)]
    public string Note { get; set; }
}