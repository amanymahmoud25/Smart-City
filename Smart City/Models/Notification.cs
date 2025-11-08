namespace Smart_City.Models;

public class Notification
{
    public int Id { get; set; }
    public int CitizenId { get; set; }
    public string Message { get; set; }
    public DateTime SentDate { get; set; }
    public virtual Citizen Citizen { get; set; }
}
