using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_City.Managers;
using Smart_City.Dtos;
using Smart_City.Models;

namespace Smart_City.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UtilityIssueController : ControllerBase
	{
		private readonly IUtilityIssueManager _manager;

		public UtilityIssueController(IUtilityIssueManager manager)
		{
			_manager = manager;
		}

		private int? GetCurrentUserId()
		{
			var idClaim = User.FindFirst("id");
			if (idClaim == null) return null;
			return int.TryParse(idClaim.Value, out var id) ? id : null;
		}

		private bool IsAdmin() => User.IsInRole("Admin");

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public ActionResult<List<UtilityIssueDto>> GetAll()
		{
			return Ok(_manager.GetAll());
		}

		[HttpGet("citizen/{citizenId:int}")]
		[Authorize(Roles = "Admin")]
		public ActionResult<List<UtilityIssueDto>> GetByCitizenId(int citizenId)
		{
			var list = _manager.GetByCitizenId(citizenId);
			if (list == null || list.Count == 0)
				return NotFound();
			return Ok(list);
		}

		[HttpGet("{id:int}")]
		[Authorize]
		public ActionResult<UtilityIssueDto> GetById(int id)
		{
			var dto = _manager.GetById(id);
			if (dto == null)
				return NotFound();
			var userId = GetCurrentUserId();
			if (!IsAdmin() && (dto.Citizen == null || dto.Citizen.Id != userId))
				return Forbid();

			return Ok(dto);
		}

		[HttpGet("mine")]
		[Authorize]
		public ActionResult<List<UtilityIssueDto>> GetMine()
		{
			var userId = GetCurrentUserId();
			if (userId == null)
				return Unauthorized();
			var list = _manager.GetByCitizenId(userId.Value);
			return Ok(list);
		}

		[HttpGet("type/{type}")]
		[Authorize]
		public ActionResult<List<UtilityIssueDto>> GetByType(UtilityIssueType type)
		{
			if (!IsAdmin())
			{
				var userId = GetCurrentUserId();
				if (userId == null)
					return Unauthorized();
				var list = _manager.GetByCitizenId(userId.Value)
					.Where(i => i.Type == type)
					.ToList();
				return Ok(list);
			}

			return Ok(_manager.GetByType(type));
		}

		[HttpGet("status/{status}")]
		[Authorize]
		public ActionResult<List<UtilityIssueDto>> GetByStatus(string status)
		{
			if (!IsAdmin())
			{
				var userId = GetCurrentUserId();
				if (userId == null)
					return Unauthorized();
				var list = _manager.GetByCitizenId(userId.Value)
					.Where(i => string.Equals(i.Status, status, StringComparison.OrdinalIgnoreCase))
					.ToList();
				return Ok(list);
			}
			return Ok(_manager.GetByStatus(status));
		}

		[HttpPost]
		[Authorize]
		public ActionResult<UtilityIssueDto> Create([FromBody] UtilityIssueCreateDto dto)
		{
			if (!ModelState.IsValid)
				return ValidationProblem(ModelState);
			var userId = GetCurrentUserId();
			if (userId == null)
				return Unauthorized();

			var created = _manager.Create(dto, userId.Value);
			if (created == null)
				return BadRequest("Failed to create issue");

			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		[HttpPatch]
		[Authorize]
		public ActionResult<UtilityIssueDto> Update([FromBody] UtilityIssueUpdateDto dto)
		{
			if (!ModelState.IsValid) return ValidationProblem(ModelState);

			var existing = _manager.GetById(dto.Id);
			if (existing == null)
				return NotFound();
			var userId = GetCurrentUserId();
			if (!IsAdmin() && (existing.Citizen == null || existing.Citizen.Id != userId))
				return Forbid();

			var updated = _manager.Update(dto);
			if (updated == null) return BadRequest("Update failed");

			return Ok(updated);
		}

		[HttpPost("{id:int}/resolve")]
		[Authorize(Roles = "Admin")]
		public IActionResult Resolve(int id)
		{
			var ok = _manager.MarkAsResolved(id);
			if (!ok)
				return NotFound();
			return NoContent();
		}

		[HttpDelete("{id:int}")]
		[Authorize]
		public IActionResult Delete(int id)
		{
			var existing = _manager.GetById(id);
			if (existing == null)
				return NotFound();

			var userId = GetCurrentUserId();
			if (!IsAdmin() && (existing.Citizen == null || existing.Citizen.Id != userId))
				return Forbid();

			var ok = _manager.Delete(id);
			if (!ok) return BadRequest();
			return NoContent();
		}
	}
}
