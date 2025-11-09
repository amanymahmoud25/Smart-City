using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_City.Models;
using Smart_City.Repositories;

namespace Smart_City.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IComplaintsRepositry _complaintRepo;
        private readonly ISuggestionsRepositories _suggestionRepo;
        private readonly IUtilityIssueRepository _utilityRepo;
        private readonly IBillRepository _billRepo;
        private readonly INotificationsRepository _notificationRepo;

        public AdminController(
            IUserRepository userRepo,
            IComplaintsRepositry complaintRepo,
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
        public IActionResult GetAllUsers() => Ok(_userRepo.GetAll());

        [HttpGet("users/{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userRepo.GetByID(id);
            return user == null ? NotFound("User not found") : Ok(user);
        }

        [HttpPut("users/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var existing = _userRepo.GetByID(id);
            if (existing == null) return NotFound("User not found");

            existing.Name = updatedUser.Name ?? existing.Name;
            existing.Email = updatedUser.Email ?? existing.Email;
            existing.Role = updatedUser.Role ?? existing.Role;

            _userRepo.Update(existing);
            return Ok("User updated successfully");
        }

        [HttpPatch("users/{id}/deactivate")]
        public IActionResult DeactivateUser(int id)
        {
            var result = _userRepo.DeactivateUser(id);
            return result ? Ok("User deactivated successfully") : NotFound("User not found");
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
        public IActionResult GetAllComplaints() => Ok(_complaintRepo.GetAll());

        [HttpGet("complaints/{id}")]
        public IActionResult GetComplaintById(int id)
        {
            var complaint = _complaintRepo.GetById(id);
            return complaint == null ? NotFound("Complaint not found") : Ok(complaint);
        }

        [HttpPut("complaints/{id}")]
        public IActionResult UpdateComplaint(int id, [FromBody] Complaint updatedComplaint)
        {
            var complaint = _complaintRepo.GetById(id);
            if (complaint == null) return NotFound("Complaint not found");

            complaint.Status = updatedComplaint.Status ?? complaint.Status;
            complaint.Description = updatedComplaint.Description ?? complaint.Description;

            var result = _complaintRepo.Update(complaint);
            return result ? Ok("Complaint updated") : BadRequest("Failed to update complaint");
        }

        [HttpPut("complaints/{id}/resolve")]
        public IActionResult ResolveComplaint(int id)
        {
            var complaint = _complaintRepo.GetById(id);
            if (complaint == null) return NotFound("Complaint not found");

            complaint.Status = "Resolved";
            var result = _complaintRepo.Update(complaint);
            return result ? Ok("Complaint resolved successfully") : BadRequest("Failed to resolve");
        }

        [HttpDelete("complaints/{id}")]
        public IActionResult DeleteComplaint(int id)
        {
            var result = _complaintRepo.Delete(id);
            return result ? Ok("Complaint deleted") : NotFound("Complaint not found");
        }

        // ===================== SUGGESTIONS =====================
        [HttpGet("suggestions")]
        public IActionResult GetAllSuggestions() => Ok(_suggestionRepo.GetAll());

        [HttpGet("suggestions/{id}")]
        public IActionResult GetSuggestionById(int id)
        {
            var suggestion = _suggestionRepo.GetById(id);
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

        // ===================== UTILITY ISSUES =====================
        [HttpGet("utility-issues")]
        public IActionResult GetAllUtilityIssues() => Ok(_utilityRepo.GetAll());

        [HttpGet("utility-issues/{id}")]
        public IActionResult GetUtilityIssueById(int id)
        {
            var issue = _utilityRepo.GetById(id);
            return issue == null ? NotFound("Issue not found") : Ok(issue);
        }

        [HttpPut("utility-issues/{id}")]
        public IActionResult UpdateUtilityIssue(int id, [FromBody] UtilityIssue updated)
        {
            var issue = _utilityRepo.GetById(id);
            if (issue == null) return NotFound("Issue not found");

            issue.Description = updated.Description ?? issue.Description;
            issue.Status = updated.Status ?? issue.Status;
            issue.Type = updated.Type;

            var result = _utilityRepo.Update(issue);
            return result ? Ok("Utility issue updated") : BadRequest("Failed to update");
        }

        [HttpPut("utility-issues/{id}/resolve")]
        public IActionResult ResolveUtilityIssue(int id)
        {
            var result = _utilityRepo.MarkAsResolved(id);
            return result ? Ok("Utility issue marked as resolved") : NotFound("Issue not found");
        }

        [HttpDelete("utility-issues/{id}")]
        public IActionResult DeleteUtilityIssue(int id)
        {
            var result = _utilityRepo.Delete(id);
            return result ? Ok("Utility issue deleted") : NotFound("Issue not found");
        }

        // ===================== BILLS =====================
        [HttpGet("bills")]
        public IActionResult GetAllBills() => Ok(_billRepo.GetAll());

        [HttpGet("bills/{id}")]
        public IActionResult GetBillById(int id)
        {
            var bill = _billRepo.GetById(id);
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

        // ===================== NOTIFICATIONS =====================
        [HttpGet("notifications")]
        public IActionResult GetAllNotifications() => Ok(_notificationRepo.GetAll());

        [HttpGet("notifications/{id}")]
        public IActionResult GetNotificationById(int id)
        {
            var notif = _notificationRepo.GetById(id);
            return notif == null ? NotFound("Notification not found") : Ok(notif);
        }

        [HttpPost("notifications")]
        public IActionResult CreateNotification([FromBody] Notification notification)
        {
            notification.SentDate = DateTime.UtcNow;
            var result = _notificationRepo.Add(notification);
            return result ? Ok("Notification created") : BadRequest("Failed to create notification");
        }

        [HttpDelete("notifications/{id}")]
        public IActionResult DeleteNotification(int id)
        {
            var result = _notificationRepo.Delete(id);
            return result ? Ok("Notification deleted") : NotFound("Notification not found");
        }

        // ===================== DASHBOARD =====================
        [HttpGet("dashboard/stats")]
        public IActionResult GetSystemStats()
        {
            var stats = new
            {
                TotalUsers = _userRepo.GetAll().Count,
                TotalComplaints = _complaintRepo.GetAll().Count,
                UnresolvedComplaints = _complaintRepo.GetUnresolved().Count,
                TotalSuggestions = _suggestionRepo.GetAll().Count,
                TotalBills = _billRepo.GetAll().Count,
                UnpaidBills = _billRepo.GetUnpaid().Count,
                TotalUtilityIssues = _utilityRepo.GetAll().Count
            };
            return Ok(stats);
        }
    }
}
