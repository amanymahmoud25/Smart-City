namespace Smart_City.Models
{
    public class Suggestion
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DateSubmitted { get; set; }
        public string Status { get; set; }

        public int CitizenId { get; set; }

        public Citizen Citizen { get; set; }

    }
}
