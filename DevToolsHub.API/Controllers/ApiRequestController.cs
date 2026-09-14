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
    public class ApiRequestController : ControllerBase
    {
        private readonly IApiRequestService _apiRequestService;
        private readonly IProjectService _projectService;
        private readonly ILogger<ApiRequestController> _logger;

        public ApiRequestController(
            IApiRequestService apiRequestService,
            IProjectService projectService,
            ILogger<ApiRequestController> logger)
        {
            _apiRequestService = apiRequestService;
            _projectService = projectService;
            _logger = logger;
        }


        // GET: api/ApiRequest
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var requests = await _apiRequestService.GetByUserIdAsync(userId.Value);

            var result = requests.Select(r => MapToResponseDto(r)).ToList();

            return Ok(result);
        }


        // GET: api/ApiRequest/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var request = await _apiRequestService.GetByIdAsync(id);

            if (request == null)
                return NotFound("API request not found.");

            if (request.UserId != userId.Value)
                return Forbid();

            var result = MapToResponseDto(request);

            return Ok(result);
        }


        // GET: api/ApiRequest/method/GET
        [HttpGet("method/{method}")]
        public async Task<IActionResult> GetByMethod(string method)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            if (string.IsNullOrWhiteSpace(method))
                return BadRequest("Method is required.");

            var requests = await _apiRequestService.GetByMethodAsync(method);

            var result = requests.Where(r => r.UserId == userId.Value).Select(r => MapToResponseDto(r)).ToList();

            return Ok(result);
        }


        // GET: api/ApiRequest/project/5
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProjectId(int projectId)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var project =await _projectService.GetByIdAsync(projectId);

            if (project == null)
                return NotFound("Project not found.");

            if (project.UserId != userId.Value)
                return Forbid();

            var requests =await _apiRequestService.GetByProjectIdAsync(projectId);

            var result = requests.Select(r => MapToResponseDto(r)).ToList();

            return Ok(result);
        }


        // GET: api/ApiRequest/page?pageNumber=1&pageSize=10
        [HttpGet("page")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1,int pageSize = 10)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            if (pageNumber < 1)
                return BadRequest("Page number must be greater than 0.");

            if (pageSize < 1 || pageSize > 100)
                return BadRequest("Page size must be between 1 and 100.");

            var requests =await _apiRequestService.GetPagedAsync(pageNumber,pageSize);

            var result = requests.Where(r => r.UserId == userId.Value).Select(r => MapToResponseDto(r)).ToList();

            return Ok(result);
        }


        // POST: api/ApiRequest
        [HttpPost]
        public async Task<IActionResult> Create(ApiRequestCreateDto dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            if (dto.ProjectId.HasValue)
            {
                var project =await _projectService.GetByIdAsync(dto.ProjectId.Value);

                if (project == null)
                    return NotFound("Project not found.");

                if (project.UserId != userId.Value)
                    return Forbid();
            }

            var apiRequest = new ApiRequest
            {
                UserId = userId.Value,
                ProjectId = dto.ProjectId,
                Name = dto.Name,
                Method = dto.Method.ToUpper(),
                Url = dto.Url,
                Headers = dto.Headers,
                Body = dto.Body,
                StatusCode = dto.StatusCode,
                ResponseBody = dto.ResponseBody
            };

            var createdRequest =
                await _apiRequestService.CreateAsync(apiRequest);

            _logger.LogInformation("API request created. RequestId: {RequestId}, UserId: {UserId}, Method: {Method}",
                createdRequest.Id,
                userId.Value,
                createdRequest.Method);

            var result = MapToResponseDto(createdRequest);

            return CreatedAtAction(nameof(GetById),new { id = result.Id },result);
        }


        // PUT: api/ApiRequest/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,ApiRequestUpdateDto dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var existingRequest =await _apiRequestService.GetByIdAsync(id);

            if (existingRequest == null)
                return NotFound("API request not found.");

            if (existingRequest.UserId != userId.Value)
                return Forbid();

            if (dto.ProjectId.HasValue)
            {
                var project = await _projectService.GetByIdAsync(dto.ProjectId.Value);

                if (project == null)
                    return NotFound("Project not found.");

                if (project.UserId != userId.Value)
                    return Forbid();
            }

            existingRequest.ProjectId = dto.ProjectId;
            existingRequest.Name = dto.Name;
            existingRequest.Method = dto.Method.ToUpper();
            existingRequest.Url = dto.Url;
            existingRequest.Headers = dto.Headers;
            existingRequest.Body = dto.Body;
            existingRequest.StatusCode = dto.StatusCode;
            existingRequest.ResponseBody = dto.ResponseBody;

            var updated = await _apiRequestService.UpdateAsync(id,existingRequest);

            if (!updated)
                return NotFound("API request not found.");

            _logger.LogInformation("API request updated. RequestId: {RequestId}, UserId: {UserId}",id,userId.Value);

            return Ok(new
            {
                success = true,
                message = "API request updated successfully."
            });
        }


        // DELETE: api/ApiRequest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var request =await _apiRequestService.GetByIdAsync(id);

            if (request == null)
                return NotFound("API request not found.");

            if (request.UserId != userId.Value)
                return Forbid();

            var deleted = await _apiRequestService.DeleteAsync(id);

            if (!deleted)
                return NotFound("API request not found.");

            _logger.LogInformation("API request deleted. RequestId: {RequestId}, UserId: {UserId}",id,userId.Value);

            return Ok(new
            {
                success = true,
                message = "API request deleted successfully."
            });
        }


        // Get User Id from JWT
        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return null;

            if (!int.TryParse(userIdClaim.Value,out int userId))
            {
                return null;
            }

            return userId;
        }


        // Entity -> DTO
        private ApiRequestResponseDto MapToResponseDto(ApiRequest request)
        {
            return new ApiRequestResponseDto
            {
                Id = request.Id,
                UserId = request.UserId,
                ProjectId = request.ProjectId,
                Name = request.Name,
                Method = request.Method,
                Url = request.Url,
                Headers = request.Headers,
                Body = request.Body,
                StatusCode = request.StatusCode,
                ResponseBody = request.ResponseBody,
                CreatedAt = request.CreatedAt
            };
        }
    }
}
