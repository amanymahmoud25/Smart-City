using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_City.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public string Type { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime IssueDate { get; set; }

        public bool IsPaid { get; set; }

        public int CitizenId { get; set; }
        public Citizen Citizen { get; set; }
    }
}
