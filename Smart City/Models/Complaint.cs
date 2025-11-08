using System.ComponentModel.DataAnnotations;

namespace Smart_City.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Title { get; set; }

        [Required, MaxLength(2000)]
        public string Description { get; set; }

        public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;

        
        public DateTime? UpdatedAt { get; set; }

        public ComplaintStatus Status { get; set; } = ComplaintStatus.Pending;

        public string? Location { get; set; }
        public string? ImageUrl { get; set; }

        
        public int CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public int? AdminId { get; set; }
        public Admin? Admin { get; set; }

        public string? AdminNote { get; set; }
    }
}
