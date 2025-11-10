using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_City.Dtos;
using Smart_City.Managers;

namespace Smart_City.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Citizen")]
    public class UtilityIssueController : ControllerBase
    {
        private readonly IUtilityIssueManager _utilityManager;

        public UtilityIssueController(IUtilityIssueManager utilityManager)
        {
            _utilityManager = utilityManager;
        }

        [HttpPost("{citizenId}")]
        public IActionResult ReportIssue(int citizenId, [FromBody] UtilityIssueCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid issue data");

            var created = _utilityManager.Create(dto, citizenId);
            return created != null ? Ok(created) : BadRequest("Failed to report issue");
        }

        [HttpGet("my/{citizenId}")]
        public IActionResult GetMyIssues(int citizenId)
        {
            var list = _utilityManager.GetByCitizenId(citizenId);
            return Ok(list);
        }
    }
}
