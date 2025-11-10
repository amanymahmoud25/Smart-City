using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_City.Models;
using Smart_City.Repositories;
using System;

namespace Smart_City.Controllers
{
    [Route("api/bills")]
    [ApiController]
    [Authorize(Roles = "Citizen")]
    public class BillsController : ControllerBase
    {
        private readonly IBillRepository _billRepo;

        public BillsController(IBillRepository billRepo)
        {
            _billRepo = billRepo;
        }

        // المواطن يشوف كل فواتيره الخاصة به
        [HttpGet("my-bills/{citizenId}")]
        public IActionResult GetMyBills(int citizenId)
        {
            var bills = _billRepo.GetByCitizenId(citizenId);
            return Ok(bills);
        }

        // المواطن يشوف فاتورة معينة
        [HttpGet("{id}")]
        public IActionResult GetBillById(int id)
        {
            var bill = _billRepo.GetById(id);
            return bill == null ? NotFound("Bill not found") : Ok(bill);
        }

        // المواطن يدفع فاتورة
        [HttpPut("{id}/pay")]
        public IActionResult PayBill(int id)
        {
            var bill = _billRepo.GetById(id);
            if (bill == null) return NotFound("Bill not found");

            var result = _billRepo.MarkAsPaid(id);
            return result ? Ok("Bill paid successfully") : BadRequest("Failed to pay bill");
        }
    }
}
