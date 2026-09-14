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
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        private readonly ILogger<FavoriteController> _logger;

        public FavoriteController(IFavoriteService favoriteService,ILogger<FavoriteController> logger)
        {
            _favoriteService = favoriteService;
            _logger = logger;
        }

        // GET: api/Favorite
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var favorites = await _favoriteService.GetAllAsync();

            return Ok(favorites);
        }

        // GET: api/Favorite/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var favorite = await _favoriteService.GetByIdAsync(id);

            if (favorite == null)
                return NotFound("Favorite not found.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            if (favorite.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            return Ok(favorite);
        }

        // GET: api/Favorite/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMyFavorites()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var favorites = await _favoriteService.GetByUserIdAsync(userId);

            return Ok(favorites);
        }

        // GET: api/Favorite/tool/5
        [HttpGet("tool/{toolId}")]
        public async Task<IActionResult> GetByUserAndTool(int toolId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var favorite =await _favoriteService.GetByUserAndToolAsync(userId,toolId);

            if (favorite == null)
                return NotFound("Favorite not found.");

            return Ok(favorite);
        }

        // POST: api/Favorite
        [HttpPost]
        public async Task<IActionResult> Create(FavoriteCreateDto dto)
        {
            var userIdClaim =User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var existingFavorite = await _favoriteService.GetByUserAndToolAsync(userId,dto.ToolId);

            if (existingFavorite != null)
                return BadRequest("Tool is already in favorites.");

            var favorite = new Favorite
            {
                UserId = userId,
                ToolId = dto.ToolId,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _favoriteService.CreateAsync(favorite);

            _logger.LogInformation("Favorite {FavoriteId} created by User {UserId}.",result.Id,userId);

            return CreatedAtAction(nameof(GetById),new { id = result.Id },result);
        }

        // DELETE: api/Favorite/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var favorite = await _favoriteService.GetByIdAsync(id);

            if (favorite == null)
                return NotFound("Favorite not found.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            if (favorite.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            var result = await _favoriteService.DeleteAsync(id);

            if (!result)
                return NotFound("Favorite not found.");

            _logger.LogInformation("Favorite {FavoriteId} deleted by User {UserId}.",id,userId);

            return Ok("Favorite deleted successfully.");
        }
    }
}
