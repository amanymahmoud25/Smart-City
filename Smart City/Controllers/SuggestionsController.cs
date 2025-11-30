using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_City.Dtos;
using Smart_City.Managers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Smart_City.Controllers
{
    [Route("api/suggestions")]
    [ApiController]
    [Authorize(Roles = "Citizen")]
    public class SuggestionsController : ControllerBase
    {
        private readonly ISuggestionManager _manager;

        public SuggestionsController(ISuggestionManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSuggestion([FromBody] SuggestionCreateDto dto, [FromQuery] int citizenId)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (citizenId <= 0)
                return BadRequest("citizenId is required");

            var created = _manager.Create(dto, citizenId); // تحتاج تعديل في الـ manager لتقبل citizenId
            if (created == null)
                return BadRequest("Failed to submit suggestion");

            return Ok("Suggestion submitted successfully");
        }

        [HttpGet("my/{citizenId}")]
        public IActionResult GetMySuggestions(int citizenId)
        {
            if (citizenId <= 0)
                return BadRequest("citizenId is required");

            var suggestions = _manager.GetByCitizenId(citizenId);
            return Ok(suggestions);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _manager.GetById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] SuggestionUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _manager.Update(id, dto);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _manager.Delete(id);
            if (!deleted)
                return NotFound();
            return Ok("Deleted successfully");
        }
    }
}
