using System.ComponentModel.DataAnnotations;

namespace Smart_City.Dtos
{
    public class BillCreateDto
    {
        [Required]
        public string Type { get; set; } // e.g. "Water", "Electricity"
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        public DateTime? IssueDate { get; set; } // optional: default DateTime.Now
    }

    public class BillUpdateDto
    {
        [Required]
        public int Id { get; set; }

        public string Type { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? IssueDate { get; set; }
        public bool? IsPaid { get; set; }
    }

    public class BillDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IsPaid { get; set; }
        public UserDto Citizen { get; set; }
    }
}
