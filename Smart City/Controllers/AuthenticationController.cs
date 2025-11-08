using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_City.Dtos;
using Smart_City.Managers;
using System.Security.Claims;

namespace Smart_City.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly IUserManager _userManager;

        public AuthenticationController(IAuthManager authManager, IUserManager userManager)
        {
            _authManager = authManager;
            _userManager = userManager;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var result = await _authManager.RegisterAsync(dto);
            if (!result.Success)
                return BadRequest(new { status = "error", message = result.Message });

            return Created(string.Empty, new { status = "success", token = result.Token, user = result.User, expiresAt = result.ExpiresAt });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var result = await _authManager.LoginAsync(dto);
            return result.Success
                ? Ok(new { status = "success", token = result.Token, user = result.User, expiresAt = result.ExpiresAt })
                : Unauthorized(new { status = "error", message = result.Message });
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var claimId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("id");
            if (!int.TryParse(claimId, out var userId))
                return Unauthorized(new { status = "error", message = "Invalid token" });

            var result = await _authManager.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);
            return result.Success
                ? Ok(new { status = "success", message = result.Message })
                : BadRequest(new { status = "error", message = result.Message });
        }

        [Authorize]
        [HttpGet("~/api/users/me")]
        public async Task<IActionResult> GetProfile()
        {
            var claimId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("id");
            if (!int.TryParse(claimId, out var userId))
                return Unauthorized(new { status = "error", message = "Invalid token" });

            var user = await _userManager.GetByIdAsync(userId);  // ✅ Async
            return user is null
                ? NotFound(new { status = "error", message = "User not found" })
                : Ok(new { status = "success", data = user });
        }

    }
}
