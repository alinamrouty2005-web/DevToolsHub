using DevToolsHub.Core.DTOs;
using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevToolsHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        // GET: api/UserRole
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userRoles = await _userRoleService.GetAllAsync();

            return Ok(userRoles);
        }

        // GET: api/UserRole/1/2
        [HttpGet("{userId}/{roleId}")]
        public async Task<IActionResult> GetById(int userId,int roleId)
        {
            var userRole =await _userRoleService.GetByIdAsync(userId, roleId);

            if (userRole == null)
                return NotFound("User role not found.");

            return Ok(userRole);
        }

        // POST: api/UserRole
        [HttpPost]
        public async Task<IActionResult> Create(UserRoleCreateDto dto)
        {
            var existingUserRole =await _userRoleService.GetByIdAsync(dto.UserId,dto.RoleId);

            if (existingUserRole != null)
                return BadRequest("User already has this role.");

            var userRole = new UserRole
            {
                UserId = dto.UserId,
                RoleId = dto.RoleId
            };

            var result =await _userRoleService.CreateAsync(userRole);

            return Ok(result);
        }

        // DELETE: api/UserRole/1/2
        [HttpDelete("{userId}/{roleId}")]
        public async Task<IActionResult> Delete(int userId,int roleId)
        {
            var result =await _userRoleService.DeleteAsync(userId, roleId);

            if (!result)
                return NotFound("User role not found.");

            return Ok("User role deleted successfully.");
        }
    }
}

