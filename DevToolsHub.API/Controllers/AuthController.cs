using DevToolsHub.Core.DTOs;
using DevToolsHub.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevToolsHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService,ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result)
                return BadRequest(new ErrorResponseDto
                {
                    Success = false,
                    Message = "Registration failed. The email may already be registered."
                });

            _logger.LogInformation("A new user registered with email {Email}.", dto.Email);

            return Ok(new
            {
                success = true,
                message = "User registered successfully."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result == null)
                return Unauthorized(new ErrorResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password."
                });

            _logger.LogInformation("User login succeeded for email {Email}.", dto.Email);

            return Ok(result);
        }
    }
}
