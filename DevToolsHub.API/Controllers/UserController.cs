using DevToolsHub.Core.DTOs;
using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevToolsHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();

            var result = users.Select(user => new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            }).ToList();

            return Ok(result);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound("User not found.");

            var result = new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            return Ok(result);
        }

        // GET: api/User/email?email=test@example.com
        [HttpGet("email")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _userService.GetByEmailAsync(email);

            if (user == null)
                return NotFound("User not found.");

            var result = new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            return Ok(result);
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = dto.Password,
                IsActive = true
            };

            var result = await _userService.CreateAsync(user);

            var response = new UserResponseDto
            {
                Id = result.Id,
                FullName = result.FullName,
                Email = result.Email,
                IsActive = result.IsActive,
                CreatedAt = result.CreatedAt
            };

            return CreatedAtAction(nameof(GetById),new { id = response.Id },response);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,UserUpdateDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                IsActive = dto.IsActive
            };

            var result = await _userService.UpdateAsync(id, user);

            if (!result)
                return NotFound("User not found.");

            var updatedUser = await _userService.GetByIdAsync(id);

            var response = new UserResponseDto
            {
                Id = updatedUser!.Id,
                FullName = updatedUser.FullName,
                Email = updatedUser.Email,
                IsActive = updatedUser.IsActive,
                CreatedAt = updatedUser.CreatedAt
            };

            return Ok(response);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);

            if (!result)
                return NotFound("User not found.");

            return Ok("User deleted successfully.");
        }
    }
}

