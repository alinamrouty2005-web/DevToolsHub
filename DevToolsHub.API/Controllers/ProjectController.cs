using DevToolsHub.Core.DTOs;
using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DevToolsHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectService projectService, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetAllAsync();
            return Ok(projects.Select(MapToResponseDto).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null) return NotFound("Project not found.");
            return Ok(MapToResponseDto(project));
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyProjects()
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();
            var projects = await _projectService.GetByUserIdAsync(userId);
            return Ok(projects.Select(MapToResponseDto).ToList());
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string name)
        {
            var projects = await _projectService.SearchAsync(name);
            return Ok(projects.Select(MapToResponseDto).ToList());
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);
            var projects = await _projectService.GetPagedAsync(pageNumber, pageSize);
            var items = projects.Select(MapToResponseDto).ToList();
            return Ok(new PagedResponseDto<ProjectResponseDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = items.Count,
                TotalPages = items.Count == 0 ? 0 : (int)Math.Ceiling(items.Count / (double)pageSize)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateDto dto)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            var project = new Project
            {
                UserId = userId,
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _projectService.CreateAsync(project);
            _logger.LogInformation("Project {ProjectId} created for User {UserId}.", result.Id, userId);
            var response = MapToResponseDto(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProjectUpdateDto dto)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();
            var existing = await _projectService.GetByIdAsync(id);
            if (existing == null) return NotFound("Project not found.");
            if (existing.UserId != userId) return Forbid();

            var project = new Project { Name = dto.Name, Description = dto.Description };
            if (!await _projectService.UpdateAsync(id, project)) return NotFound("Project not found.");

            var updated = await _projectService.GetByIdAsync(id);
            _logger.LogInformation("Project {ProjectId} updated by User {UserId}.", id, userId);
            return Ok(MapToResponseDto(updated!));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();
            var project = await _projectService.GetByIdAsync(id);
            if (project == null) return NotFound("Project not found.");
            if (project.UserId != userId) return Forbid();
            if (!await _projectService.DeleteAsync(id)) return NotFound("Project not found.");

            _logger.LogInformation("Project {ProjectId} deleted by User {UserId}.", id, userId);
            return Ok("Project deleted successfully.");
        }

        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null && int.TryParse(claim.Value, out userId);
        }

        private static ProjectResponseDto MapToResponseDto(Project project)
        {
            return new ProjectResponseDto
            {
                Id = project.Id,
                UserId = project.UserId,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            };
        }
    }
}
