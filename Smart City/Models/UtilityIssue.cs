namespace Smart_City.Models
{
    public class UtilityIssue
    {
        public int Id { get; set; }
        public UtilityIssueType Type { get; set; }
        public string Description { get; set; }

        public DateTime ReportDate { get; set; }
        public string Status { get; set; }

        public int CitizenId { get; set; }
        public Citizen Citizen { get; set; }
    }
}
