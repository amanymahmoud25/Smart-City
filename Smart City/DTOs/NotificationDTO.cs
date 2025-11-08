using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Smart_City.Dtos;

public class NotificationDto
{
	public int Id { get; set; }
	public string Message { get; set; }
	public DateTime SentDate { get; set; }
	public UserBriefDto Citizen { get; set; } 
}

public class NotificationCreateDto
{
    [Required]
    public string Message { get; set; }
    public int? CitizenId { get; set; } 
}

public class BroadcastCreateDto
{
	[Required]
	public string Message { get; set; }
}
