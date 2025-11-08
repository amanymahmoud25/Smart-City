using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_City.Dtos;
using Smart_City.Managers;
using System.Security.Claims;

namespace Smart_City.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComplaintsController : ControllerBase
{
    private readonly IComplaintManager _svc;
    public ComplaintsController(IComplaintManager svc) { _svc = svc; }

    [Authorize(Roles = "Citizen")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ComplaintCreateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("id");
        if (!int.TryParse(idStr, out var citizenId))
            return Unauthorized(new { status = "error", message = "Unauthorized" });

        var created = await _svc.CreateAsync(dto, citizenId);

        return created is null
            ? BadRequest(new { status = "error", message = "Failed to create" })
            : CreatedAtAction(nameof(GetById), new { id = created.Id, version = "1" },
                new { status = "success", data = created });
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet("mine")]
    public async Task<IActionResult> Mine([FromQuery] string? status, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("id");
        if (!int.TryParse(idStr, out var citizenId))
            return Unauthorized(new { status = "error", message = "Unauthorized" });

        var list = await _svc.GetByCitizenAsync(citizenId, status, from, to);
        return Ok(new { status = "success", count = list.Count, data = list });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? status,
        [FromQuery] int? citizenId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(status) && !citizenId.HasValue && !from.HasValue && !to.HasValue)
        {
            var list = await _svc.GetAllAsync();
            return Ok(new { status = "success", count = list.Count, data = list });
        }

        var paged = await _svc.GetAllAsync(status, citizenId, from, to, page, pageSize);
        return Ok(new { status = "success", data = paged });
    }

    [Authorize(Roles = "Citizen,Admin")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _svc.GetByIdAsync(id);
        if (item is null) return NotFound(new { status = "error", message = "Not found" });

        if (User.IsInRole("Citizen"))
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("id");
            if (!int.TryParse(idStr, out var citizenId) || item.CitizenId != citizenId)
                return Forbid();
        }

        return Ok(new { status = "success", data = item });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] ComplaintUpdateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var adminStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("id");
        if (!int.TryParse(adminStr, out var adminId))
            return Unauthorized(new { status = "error", message = "Unauthorized" });

        var updated = await _svc.UpdateStatusAsync(id, dto.Status, adminId, dto.Note);

        return updated is null
            ? BadRequest(new { status = "error", message = "Invalid transition or not found" })
            : Ok(new { status = "success", message = "Updated", data = updated });
    }
}
