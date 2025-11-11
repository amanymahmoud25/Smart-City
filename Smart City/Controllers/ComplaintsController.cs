using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_City.Dtos;
using Smart_City.Managers;
using System.Threading.Tasks;

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
        public async Task<IActionResult> CreateComplaint([FromBody] ComplaintCreateDto dto, [FromQuery] int citizenId)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (citizenId <= 0)
                return BadRequest("citizenId is required");

            var created = await _manager.CreateAsync(dto, citizenId);
            if (created == null)
                return BadRequest("Failed to submit complaint");

            return Ok("Complaint submitted successfully");
        }

        [HttpGet("my/{citizenId}")]
        public async Task<IActionResult> GetMyComplaints(int citizenId)
        {
            var complaints = await _manager.GetByCitizenAsync(citizenId, null, null, null);
            return Ok(complaints);
        }
    }
}
