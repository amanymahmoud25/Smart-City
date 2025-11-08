using Microsoft.AspNetCore.Mvc;
using Smart_City.Managers;
using Smart_City.Dtos;

namespace Smart_City.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuggestionsController : ControllerBase
    {
        private readonly ISuggestionManager _manager;

        public SuggestionsController(ISuggestionManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public ActionResult<List<SuggestionDto>> GetAll()
        {
            return Ok(_manager.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<SuggestionDto> GetById(int id)
        {
            var result = _manager.GetById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("citizen/{citizenId}")]
        public ActionResult<List<SuggestionDto>> GetByCitizenId(int citizenId)
        {
            return Ok(_manager.GetByCitizenId(citizenId));
        }

        [HttpPost]
        public ActionResult<SuggestionDto> Create([FromBody] SuggestionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _manager.Create(dto);
            if (result == null) return BadRequest();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public ActionResult<SuggestionDto> Update(int id, [FromBody] SuggestionUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _manager.Update(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _manager.Delete(id);
            if (!deleted) return NotFound();
            return Ok("Deleted successfully");
        }
    }
}
