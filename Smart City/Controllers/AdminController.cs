using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_City.Dtos;
using Smart_City.Models;
using Smart_City.Repositories;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Smart_City.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IComplaintRepositry _complaintRepo;
        private readonly ISuggestionsRepositories _suggestionRepo;
        private readonly IUtilityIssueRepository _utilityRepo;
        private readonly IBillRepository _billRepo;
        private readonly INotificationsRepository _notificationRepo;

        public AdminController(
            IUserRepository userRepo,
            IComplaintRepositry complaintRepo,
            ISuggestionsRepositories suggestionRepo,
            IUtilityIssueRepository utilityRepo,
            IBillRepository billRepo,
            INotificationsRepository notificationRepo)
        {
            _userRepo = userRepo;
            _complaintRepo = complaintRepo;
            _suggestionRepo = suggestionRepo;
            _utilityRepo = utilityRepo;
            _billRepo = billRepo;
            _notificationRepo = notificationRepo;
        }

        // ===================== USERS =====================
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var data = _userRepo.GetAll()
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Phone,
                    u.Role,
                    u.IsActive,
                    u.CreatedAt
                })
                .ToList();

            return Ok(data);
        }

        [HttpGet("users/{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userRepo.GetAll()
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Phone,
                    u.Address,
                    u.Role,
                    u.IsActive,
                    u.CreatedAt
                })
                .FirstOrDefault();

            return user == null ? NotFound("User not found") : Ok(user);
        }

        [HttpPut("users/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var existing = _userRepo.GetById(id);
            if (existing == null) return NotFound("User not found");

            existing.Name = updatedUser.Name ?? existing.Name;
            existing.Email = updatedUser.Email ?? existing.Email;
            existing.Role = updatedUser.Role ?? existing.Role;

            _userRepo.Update(existing);
            return Ok("User updated successfully");
        }

        [HttpPut("users/{id}/promote")]
        public IActionResult PromoteToAdmin(int id)
        {
            var result = _userRepo.PromoteToAdmin(id);
            return result ? Ok("User promoted to Admin") : NotFound("User not found");
        }

        [HttpDelete("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var result = _userRepo.Delete(id);
            return result ? Ok("User deleted successfully") : NotFound("User not found");
        }

        // ===================== COMPLAINTS =====================
        [HttpGet("complaints")]
        public async Task<IActionResult> GetAllComplaints()
        {
            var data = (await _complaintRepo.GetAllAsync())
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.Status,
                    c.DateSubmitted,
                    c.CitizenId
                })
                .ToList();

            return Ok(data);
        }

        [HttpGet("complaints/{id}")]
        public async Task<IActionResult> GetComplaintById(int id)
        {
            var complaint = (await _complaintRepo.GetAllAsync())
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.Description,
                    c.Status,
                    c.Location,
                    c.ImageUrl,
                    c.DateSubmitted,
                    c.UpdatedAt,
                    c.CitizenId
                })
                .FirstOrDefault();

            return complaint == null ? NotFound("Complaint not found") : Ok(complaint);
        }

        [HttpPut("complaints/{id}")]
        public async Task<IActionResult> UpdateComplaint(int id, [FromBody] ComplaintUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var existing = await _complaintRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound("Complaint not found");

            if (!string.IsNullOrWhiteSpace(dto.Title))
                existing.Title = dto.Title;
            if (!string.IsNullOrWhiteSpace(dto.Description))
                existing.Description = dto.Description;
            if (!string.IsNullOrWhiteSpace(dto.Location))
                existing.Location = dto.Location;
            if (!string.IsNullOrWhiteSpace(dto.ImageUrl))
                existing.ImageUrl = dto.ImageUrl;

            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _complaintRepo.UpdateAsync(existing);
            return updated ? Ok("Complaint updated successfully") : BadRequest("Failed to update complaint");
        }

        [HttpPut("complaints/{id}/status")]
        public async Task<IActionResult> UpdateComplaintStatus(int id, [FromBody] ComplaintStatusUpdateDto dto)
        {
            var claimId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("id");
            if (!int.TryParse(claimId, out var adminId))
                return Unauthorized("Invalid token");

            var updated = await _complaintRepo.UpdateStatusAsync(id, dto.Status, adminId);
            return updated ? Ok("Complaint status updated") : NotFound("Complaint not found");
        }

        [HttpDelete("complaints/{id}")]
        public async Task<IActionResult> DeleteComplaint(int id)
        {
            var result = await _complaintRepo.DeleteAsync(id);
            return result ? Ok("Complaint deleted") : NotFound("Complaint not found");
        }

        // ===================== SUGGESTIONS =====================
        [HttpGet("suggestions")]
        public IActionResult GetAllSuggestions()
        {
            var data = _suggestionRepo.GetAll()
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Status,
                    s.DateSubmitted,
                    s.CitizenId
                })
                .ToList();

            return Ok(data);
        }

        [HttpGet("suggestions/{id}")]
        public IActionResult GetSuggestionById(int id)
        {
            var suggestion = _suggestionRepo.GetAll()
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Description,
                    s.Status,
                    s.DateSubmitted,
                    s.CitizenId
                })
                .FirstOrDefault();

            return suggestion == null ? NotFound("Suggestion not found") : Ok(suggestion);
        }

        [HttpPut("suggestions/{id}")]
        public IActionResult UpdateSuggestion(int id, [FromBody] Suggestion updated)
        {
            var suggestion = _suggestionRepo.GetById(id);
            if (suggestion == null) return NotFound("Suggestion not found");

            suggestion.Title = updated.Title ?? suggestion.Title;
            suggestion.Description = updated.Description ?? suggestion.Description;
            suggestion.Status = updated.Status ?? suggestion.Status;

            var result = _suggestionRepo.Update(suggestion);
            return result ? Ok("Suggestion updated") : BadRequest("Failed to update");
        }

        [HttpDelete("suggestions/{id}")]
        public IActionResult DeleteSuggestion(int id)
        {
            var result = _suggestionRepo.Delete(id);
            return result ? Ok("Suggestion deleted") : NotFound("Suggestion not found");
        }

        // ===================== BILLS =====================
        [HttpGet("bills")]
        public IActionResult GetAllBills()
        {
            var data = _billRepo.GetAll()
                .Select(b => new
                {
                    b.Id,
                    b.Type,
                    b.Amount,
                    b.IssueDate,
                    b.IsPaid,
                    b.CitizenId
                })
                .ToList();

            return Ok(data);
        }

        [HttpGet("bills/{id}")]
        public IActionResult GetBillById(int id)
        {
            var bill = _billRepo.GetAll()
                .Where(b => b.Id == id)
                .Select(b => new
                {
                    b.Id,
                    b.Type,
                    b.Amount,
                    b.IssueDate,
                    b.IsPaid,
                    b.CitizenId
                })
                .FirstOrDefault();

            return bill == null ? NotFound("Bill not found") : Ok(bill);
        }

        [HttpPut("bills/{id}/paid")]
        public IActionResult MarkBillAsPaid(int id)
        {
            var result = _billRepo.MarkAsPaid(id);
            return result ? Ok("Bill marked as paid") : NotFound("Bill not found");
        }

        [HttpDelete("bills/{id}")]
        public IActionResult DeleteBill(int id)
        {
            var result = _billRepo.Delete(id);
            return result ? Ok("Bill deleted") : NotFound("Bill not found");
        }

        // ===================== UTILITY ISSUES =====================
        [HttpGet("utility-issues")]
        public IActionResult GetAllUtilityIssues()
        {
            var data = _utilityRepo.GetAll()
                .Select(u => new
                {
                    u.Id,
                    u.Type,
                    u.Status,
                    u.ReportDate,
                    u.CitizenId
                })
                .ToList();

            return Ok(data);
        }

        [HttpGet("utility-issues/{id}")]
        public IActionResult GetUtilityIssueById(int id)
        {
            var issue = _utilityRepo.GetAll()
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.Type,
                    u.Description,
                    u.Status,
                    u.ReportDate,
                    u.CitizenId
                })
                .FirstOrDefault();

            return issue == null ? NotFound("Issue not found") : Ok(issue);
        }

        // ===================== NOTIFICATIONS =====================
        [HttpGet("notifications")]
        public IActionResult GetAllNotifications()
        {
            var data = _notificationRepo.GetAll()
                .Select(n => new
                {
                    n.Id,
                    n.Message,
                    n.SentDate,
                    n.CitizenId
                })
                .ToList();

            return Ok(data);
        }

        [HttpGet("notifications/{id}")]
        public IActionResult GetNotificationById(int id)
        {
            var notif = _notificationRepo.GetAll()
                .Where(n => n.Id == id)
                .Select(n => new
                {
                    n.Id,
                    n.Message,
                    n.SentDate,
                    n.CitizenId
                })
                .FirstOrDefault();

            return notif == null ? NotFound("Notification not found") : Ok(notif);
        }

        [HttpPost("notifications")]
        public IActionResult CreateNotification([FromBody] CreateNotificationDto dto)
        {
            var notification = new Notification
            {
                CitizenId = dto.CitizenId,
                Message = dto.Message,
                SentDate = DateTime.UtcNow
            };

            var result = _notificationRepo.Add(notification);
            return result ? Ok("Notification created") : BadRequest("Failed to create notification");
        }

        // ===================== DASHBOARD =====================
        [HttpGet("dashboard/stats")]
        public async Task<IActionResult> GetSystemStats()
        {
            var stats = new
            {
                TotalUsers = _userRepo.GetAll().Count,
                TotalComplaints = (await _complaintRepo.GetAllAsync()).Count,
                TotalSuggestions = _suggestionRepo.GetAll().Count,
                TotalBills = _billRepo.GetAll().Count,
                TotalUtilityIssues = _utilityRepo.GetAll().Count,
                TotalNotifications = _notificationRepo.GetAll().Count
            };

            return Ok(stats);
        }
    }
}
