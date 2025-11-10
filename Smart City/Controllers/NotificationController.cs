using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_City.Managers;

namespace Smart_City.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Citizen")]
	public class NotificationController : ControllerBase
	{
		private readonly INotificationManager _notificationManager;

		public NotificationController(INotificationManager notificationManager)
		{
			_notificationManager = notificationManager;
		}

		[HttpGet("my/{citizenId}")]
		public IActionResult GetMyNotifications(int citizenId)
		{
			var list = _notificationManager.GetByCitizenId(citizenId);
			return Ok(list);
		}
	}
}
