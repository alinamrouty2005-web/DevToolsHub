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
    public class ToolHistoryController : ControllerBase
    {
        private readonly IToolHistoryService _toolHistoryService;
        private readonly ILogger<ToolHistoryController> _logger;

        public ToolHistoryController(IToolHistoryService toolHistoryService,ILogger<ToolHistoryController> logger)
        {
            _toolHistoryService = toolHistoryService;
            _logger = logger;
        }

        // GET: api/ToolHistory
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var histories = await _toolHistoryService.GetAllAsync();

            return Ok(histories);
        }

        // GET: api/ToolHistory/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var history =await _toolHistoryService.GetByIdAsync(id);

            if (history == null)
                return NotFound("Tool history not found.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            if (history.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            return Ok(history);
        }

        // GET: api/ToolHistory/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMyHistory()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var histories =await _toolHistoryService.GetByUserIdAsync(userId);

            return Ok(histories);
        }

        // GET: api/ToolHistory/tool/5
        [HttpGet("tool/{toolId}")]
        public async Task<IActionResult> GetByToolId(int toolId)
        {
            var histories = await _toolHistoryService.GetByToolIdAsync(toolId);

            return Ok(histories);
        }

        // GET: api/ToolHistory/project/5
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProjectId(int projectId)
        {
            var histories =await _toolHistoryService.GetByProjectIdAsync(projectId);

            return Ok(histories);
        }

        // GET: api/ToolHistory/paged?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1,int pageSize = 10)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            if (pageSize > 100)
                pageSize = 100;

            var histories = await _toolHistoryService.GetPagedAsync(pageNumber, pageSize);

            return Ok(histories);
        }

        // POST: api/ToolHistory
        [HttpPost]
        public async Task<IActionResult> Create(ToolHistoryCreateDto dto)
        {
            var userIdClaim =User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var toolHistory = new ToolHistory
            {
                UserId = userId,
                ToolId = dto.ToolId,
                ProjectId = dto.ProjectId,
                InputData = dto.InputData,
                OutputData = dto.OutputData,
                ExecutedAt = DateTime.UtcNow
            };

            var result = await _toolHistoryService.CreateAsync(toolHistory);

            _logger.LogInformation("Tool history {HistoryId} created by User {UserId}.",result.Id,userId);

            return CreatedAtAction(nameof(GetById),new { id = result.Id },result);
        }

        // PUT: api/ToolHistory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ToolHistoryCreateDto dto)
        {
            var history = await _toolHistoryService.GetByIdAsync(id);

            if (history == null)
                return NotFound("Tool history not found.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            if (history.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            var updatedHistory = new ToolHistory
            {
                ToolId = dto.ToolId,
                ProjectId = dto.ProjectId,
                InputData = dto.InputData,
                OutputData = dto.OutputData
            };

            var result = await _toolHistoryService.UpdateAsync(id, updatedHistory);

            if (!result)
                return NotFound("Tool history not found.");

            return Ok(await _toolHistoryService.GetByIdAsync(id));
        }

        // DELETE: api/ToolHistory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var history = await _toolHistoryService.GetByIdAsync(id);

            if (history == null)
                return NotFound("Tool history not found.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            if (history.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            var result = await _toolHistoryService.DeleteAsync(id);

            if (!result)
                return NotFound("Tool history not found.");

            _logger.LogInformation("Tool history {HistoryId} deleted by User {UserId}.",id,userId);

            return Ok("Tool history deleted successfully.");
        }
    }
}
