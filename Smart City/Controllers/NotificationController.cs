using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Smart_City.Dtos;
using Smart_City.Managers;

namespace Smart_City.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
	private readonly INotificationManager _notificationManager;

	public NotificationController(INotificationManager notificationManager)
	{
		_notificationManager = notificationManager;
	}

	// Helper: current user id from JWT
	private int? GetCurrentUserId()
	{
		var idClaim = User.FindFirst("id");
		if (idClaim == null)
			return null;
		return int.TryParse(idClaim.Value, out var id) ? id : null;
	}

	private bool IsAdmin() => User.IsInRole("Admin");

	[HttpGet]
	[Authorize(Roles = "Admin")]
	public ActionResult<List<NotificationDto>> GetAll()
	{
		return Ok(_notificationManager.GetAll());
	}

	[HttpGet("mine")]
	[Authorize]
	public ActionResult<List<NotificationDto>> GetMine()
	{
		var userId = GetCurrentUserId();
		if (userId == null)
			return Unauthorized();
		return Ok(_notificationManager.GetByCitizenId(userId.Value));
	}

	[HttpGet("citizens/{citizenId:int}")]
	[Authorize(Roles = "Admin")]
	public ActionResult<List<NotificationDto>> GetByCitizenId(int citizenId)
	{
		var dto = _notificationManager.GetByCitizenId(citizenId);
		if (dto.IsNullOrEmpty())
			return NotFound();
		return Ok(dto);
	}


	[HttpGet("citizens/{citizenName:alpha}")]
	[Authorize(Roles = "Admin")]
	public ActionResult<List<NotificationDto>> GetByCitizenName(string citizenName)
	{
		var dto = _notificationManager.GetByCitizenName(citizenName);
		if (dto.IsNullOrEmpty())
			return NotFound();
		return Ok(dto);
	}

	[HttpGet("{id:int}")]
	[Authorize]
	public ActionResult<NotificationDto> GetById(int id)
	{
		var dto = _notificationManager.GetById(id);
		if (dto == null)
			return NotFound();

		var userId = GetCurrentUserId();
		if (!IsAdmin() && (dto.Citizen == null || dto.Citizen.Id != userId))
			return Forbid();
		return Ok(dto);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public IActionResult Create([FromBody] NotificationCreateDto createDto)
	{
		if (!ModelState.IsValid)
			return ValidationProblem(ModelState);

		var ok = _notificationManager.Create(createDto);
		if (!ok)
			return BadRequest("Failed to create notification");
		return Created(string.Empty, null);
	}

	[HttpPost("broadcast")]
	[Authorize(Roles = "Admin")]
	public IActionResult Broadcast([FromBody] BroadcastCreateDto createDto)
	{
		if (string.IsNullOrWhiteSpace(createDto.Message))
			return BadRequest("Message required");

		var ok = _notificationManager.Broadcast(createDto.Message);
		if (!ok)
			return BadRequest("Failed to broadcast notification");
		return Accepted();
	}

	[HttpDelete("{id:int}")]
	[Authorize]
	public IActionResult Delete(int id)
	{
		var dto = _notificationManager.GetById(id);
		if (dto == null)
			return NotFound();

		var userId = GetCurrentUserId();
		if (!IsAdmin() && (dto.Citizen == null || dto.Citizen.Id != userId))
			return Forbid();

		var ok = _notificationManager.Delete(id);
		if (!ok)
			return BadRequest("Failed to delete notification");
		return NoContent();
	}
}
