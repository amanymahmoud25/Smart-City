using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_City.Models;
using Smart_City.Repositories;
using System.Threading.Tasks;

namespace Smart_City.Controllers
{
    [Route("api/complaints")]
    [ApiController]
    [Authorize(Roles = "Citizen")]
    public class ComplaintsController : ControllerBase
    {
        private readonly IComplaintRepositry _complaintRepo;

        public ComplaintsController(IComplaintRepositry complaintRepo)
        {
            _complaintRepo = complaintRepo;
        }

        // المواطن يقدّم شكوى جديدة
        [HttpPost]
        public async Task<IActionResult> CreateComplaint([FromBody] Complaint complaint)
        {
            if (complaint == null) return BadRequest("Invalid complaint data");

            complaint.Status = ComplaintStatus.Pending;
            var result = await _complaintRepo.AddAsync(complaint);

            return result ? Ok("Complaint submitted successfully") : BadRequest("Failed to submit complaint");
        }

        // المواطن يشوف كل الشكاوى الخاصة به
        [HttpGet("my/{citizenId}")]
        public async Task<IActionResult> GetMyComplaints(int citizenId)
        {
            var complaints = await _complaintRepo.GetByCitizenIdAsync(citizenId);
            return Ok(complaints);
        }
    }
}
