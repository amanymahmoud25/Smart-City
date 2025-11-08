    namespace Smart_City.Models
    {
        public class Admin : User
        {

            public ICollection<Complaint> ManagedComplaints { get; set; }
            public ICollection<Suggestion> ManagedSuggestions { get; set; }

        }
    }
