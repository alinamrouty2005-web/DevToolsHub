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
    public class ToolController : ControllerBase
    {
        private readonly IToolService _toolService;
        private readonly ILogger<ToolController> _logger;

        public ToolController(IToolService toolService, ILogger<ToolController> logger)
        {
            _toolService = toolService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var tools = await _toolService.GetAllAsync();
            return Ok(tools.Select(MapToResponseDto).ToList());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var tool = await _toolService.GetByIdAsync(id);
            if (tool == null) return NotFound("Tool not found.");
            return Ok(MapToResponseDto(tool));
        }

        [HttpGet("name")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByName(string name)
        {
            var tool = await _toolService.GetByNameAsync(name);
            if (tool == null) return NotFound("Tool not found.");
            return Ok(MapToResponseDto(tool));
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string name)
        {
            var tools = await _toolService.SearchAsync(name);
            return Ok(tools.Select(MapToResponseDto).ToList());
        }

        [HttpGet("category")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var tools = await _toolService.GetByCategoryAsync(category);
            return Ok(tools.Select(MapToResponseDto).ToList());
        }

        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActive()
        {
            var tools = await _toolService.GetActiveAsync();
            return Ok(tools.Select(MapToResponseDto).ToList());
        }

        [HttpGet("paged")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaged(
            int pageNumber = 1,
            int pageSize = 10,
            string? search = null,
            string? category = null,
            string? sortBy = null,
            bool sortDescending = false)
        {
            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);
            var tools = await _toolService.GetPagedAsync(pageNumber, pageSize, search, category, sortBy, sortDescending);
            var items = tools.Select(MapToResponseDto).ToList();
            return Ok(new PagedResponseDto<ToolResponseDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = items.Count,
                TotalPages = items.Count == 0 ? 0 : (int)Math.Ceiling(items.Count / (double)pageSize)
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ToolCreateDto dto)
        {
            var existing = await _toolService.GetByNameAsync(dto.Name);
            if (existing != null) return BadRequest("Tool already exists.");

            var tool = new Tool
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _toolService.CreateAsync(tool);
            _logger.LogInformation("Tool {ToolId} created.", result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, MapToResponseDto(result));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, ToolUpdateDto dto)
        {
            var tool = new Tool
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                IsActive = dto.IsActive
            };

            if (!await _toolService.UpdateAsync(id, tool)) return NotFound("Tool not found.");
            var updated = await _toolService.GetByIdAsync(id);
            _logger.LogInformation("Tool {ToolId} updated.", id);
            return Ok(MapToResponseDto(updated!));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _toolService.DeleteAsync(id)) return NotFound("Tool not found.");
            _logger.LogInformation("Tool {ToolId} deleted.", id);
            return Ok("Tool deleted successfully.");
        }

        private static ToolResponseDto MapToResponseDto(Tool tool)
        {
            return new ToolResponseDto
            {
                Id = tool.Id,
                Name = tool.Name,
                Description = tool.Description,
                Category = tool.Category,
                IsActive = tool.IsActive,
                CreatedAt = tool.CreatedAt
            };
        }
    }
}
