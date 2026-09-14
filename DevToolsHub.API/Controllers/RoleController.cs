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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles.Select(MapToResponseDto).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null) return NotFound("Role not found.");
            return Ok(MapToResponseDto(role));
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetByName(string name)
        {
            var role = await _roleService.GetByNameAsync(name);
            if (role == null) return NotFound("Role not found.");
            return Ok(MapToResponseDto(role));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(RoleCreateDto dto)
        {
            if (await _roleService.GetByNameAsync(dto.Name) != null)
                return BadRequest("Role already exists.");

            var role = new Role { Name = dto.Name, Description = dto.Description };
            var result = await _roleService.CreateAsync(role);
            _logger.LogInformation("Role {RoleId} created.", result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, MapToResponseDto(result));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, RoleUpdateDto dto)
        {
            var role = new Role { Name = dto.Name, Description = dto.Description };
            if (!await _roleService.UpdateAsync(id, role)) return NotFound("Role not found.");
            var updated = await _roleService.GetByIdAsync(id);
            _logger.LogInformation("Role {RoleId} updated.", id);
            return Ok(MapToResponseDto(updated!));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _roleService.DeleteAsync(id)) return NotFound("Role not found.");
            _logger.LogInformation("Role {RoleId} deleted.", id);
            return Ok("Role deleted successfully.");
        }

        private static RoleResponseDto MapToResponseDto(Role role)
        {
            return new RoleResponseDto { Id = role.Id, Name = role.Name, Description = role.Description };
        }
    }
}
