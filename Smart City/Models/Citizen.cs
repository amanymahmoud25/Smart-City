namespace Smart_City.Models
{
    public class Citizen : User 
    {
      

        public string Address { get; set; }


       public ICollection<Complaint> Complaints { get; set; }

        public ICollection<Suggestion> Suggestions { get; set; }
        public ICollection<Bill> Bills { get; set; }
        public ICollection<UtilityIssue> UtilityIssues { get; set; }
        public ICollection<Notification> Notifications { get; set; }

    }
}
