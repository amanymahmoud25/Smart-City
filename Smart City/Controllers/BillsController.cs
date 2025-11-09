using Microsoft.AspNetCore.Mvc;
using Smart_City.Dtos;
using Smart_City.Managers;
using Microsoft.AspNetCore.Authorization;

namespace Smart_City.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillsController : ControllerBase
    {
        private readonly IBillManager _billManager;
        private readonly INotificationManager _notificationManager;

        public BillsController(IBillManager billManager, INotificationManager notificationManager)
        {
            _billManager = billManager;
            _notificationManager = notificationManager;
        }

        // ============================================================
        // GET: api/bills
        // [Admin] Get all bills in the system
        // ============================================================
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            var bills = _billManager.GetAll();
            if (bills == null || !bills.Any())
                return NotFound("No bills found.");

            return Ok(bills);
        }

        // ============================================================
        // GET: api/bills/{id}
        // [Admin] Get a single bill by ID
        // ============================================================
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetById(int id)
        {
            var bill = _billManager.GetById(id);
            if (bill == null)
                return NotFound($"Bill with ID {id} not found.");

            return Ok(bill);
        }

        // ============================================================
        // GET: api/bills/citizen/{citizenId}
        // [Citizen or Admin] Get all bills for a specific citizen
        // ============================================================
        [HttpGet("citizen/{citizenId}")]
        [Authorize(Roles = "Admin,Citizen")]
        public IActionResult GetByCitizenId(int citizenId)
        {
            // If user is a Citizen, ensure they only access their own bills
            var userRole = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (userRole == "Citizen" && userIdClaim != null && userIdClaim != citizenId.ToString())
                return Forbid("Citizens can only view their own bills.");

            var bills = _billManager.GetByCitizenId(citizenId);
            if (bills == null || !bills.Any())
                return NotFound("No bills found for this citizen.");

            return Ok(bills);
        }

        // ============================================================
        // POST: api/bills
        // [Admin] Create a new bill for a citizen
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] BillCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = _billManager.Create(dto);
            if (created == null)
                return BadRequest("Failed to create bill. Check data or CitizenId.");

            // Notify citizen about new bill
            if (created.Citizen != null)
            {
                _notificationManager.CreateForCitizen(created.Citizen.Id, $"A new {created.Type} bill has been issued.");
            }

            return Ok(created);
        }

        // ============================================================
        // PUT: api/bills/{id}
        // [Admin] Update an existing bill
        // ============================================================
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, [FromBody] BillUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = _billManager.Update(id, dto);
            if (updated == null)
                return NotFound($"Bill with ID {id} not found.");

            return Ok(updated);
        }

        // ============================================================
        // PUT: api/bills/{id}/mark-paid
        // [Admin] Mark a bill as paid (simulate payment confirmation)
        // ============================================================
        [HttpPut("{id}/mark-paid")]
        [Authorize(Roles = "Admin")]
        public IActionResult MarkAsPaid(int id)
        {
            var success = _billManager.MarkAsPaid(id);
            if (!success)
                return NotFound($"Bill with ID {id} not found or could not be updated.");

            // Send notification to citizen
            var bill = _billManager.GetById(id);
            if (bill?.Citizen != null)
            {
                _notificationManager.CreateForCitizen(
                    bill.Citizen.Id,
                    $"Your {bill.Type} bill has been marked as paid. Thank you!"
                );
            }

            return Ok(new { Message = "Bill marked as paid successfully." });
        }

        // ============================================================
        // DELETE: api/bills/{id}
        // [Admin] Delete a bill
        // ============================================================
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var success = _billManager.Delete(id);
            if (!success)
                return NotFound($"Bill with ID {id} not found.");

            return Ok(new { Message = "Bill deleted successfully." });
        }

        // ============================================================
        // GET: api/bills/paid
        // [Admin] Get all paid bills
        // ============================================================
        [HttpGet("paid")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPaid()
        {
            var bills = _billManager.GetPaid();
            return Ok(bills);
        }

        // ============================================================
        // GET: api/bills/unpaid
        // [Admin] Get all unpaid bills
        // ============================================================
        [HttpGet("unpaid")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUnpaid()
        {
            var bills = _billManager.GetUnpaid();
            return Ok(bills);
        }

        // ============================================================
        // GET: api/bills/type/{type}
        // [Admin] Get all bills filtered by type (e.g., Water, Electricity)
        // ============================================================
        [HttpGet("type/{type}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetByType(string type)
        {
            if (string.IsNullOrEmpty(type))
                return BadRequest("Type cannot be empty.");

            var allBills = _billManager.GetAll();
            var filtered = allBills.Where(b => b.Type.Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!filtered.Any())
                return NotFound($"No bills found for type: {type}");

            return Ok(filtered);
        }
    }
}
