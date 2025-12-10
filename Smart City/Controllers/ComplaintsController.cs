using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_City.Dtos;
using Smart_City.Managers;
using System.Threading.Tasks;
using System.Security.Claims;


namespace Smart_City.Controllers
{
    [Route("api/complaints")]
    [ApiController]
    [Authorize(Roles = "Citizen")]
    public class ComplaintsController : ControllerBase
    {
        private readonly IComplaintManager _manager;

        public ComplaintsController(IComplaintManager manager)
        {
            _manager = manager;
        }


        [HttpPost]
        public async Task<IActionResult> CreateComplaint([FromBody] ComplaintCreateDto dto)
        {
            var claimId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? User.FindFirstValue("id");

            if (!int.TryParse(claimId, out var citizenId))
                return Unauthorized("Invalid token");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var created = await _manager.CreateAsync(dto, citizenId);
            if (created == null)
                return BadRequest("Failed to submit complaint");

            return Ok("Complaint submitted successfully");
        }



        [HttpGet("my/citizenId")]
        public async Task<IActionResult> GetMyComplaints()
        {
            var claimId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? User.FindFirstValue("id");

            if (!int.TryParse(claimId, out var citizenId))
                return Unauthorized("Invalid token");

            var complaints = await _manager.GetByCitizenAsync(citizenId, null, null, null);

            return Ok(complaints);
        }
    }
}
