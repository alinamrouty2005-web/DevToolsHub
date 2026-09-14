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
    public class CollectionController : ControllerBase
    {
        private readonly ICollectionService _collectionService;
        private readonly ILogger<CollectionController> _logger;

        public CollectionController(ICollectionService collectionService,ILogger<CollectionController> logger)
        {
            _collectionService = collectionService;
            _logger = logger;
        }

        // GET: api/Collection
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var collections = await _collectionService.GetAllAsync();

            return Ok(collections);
        }

        // GET: api/Collection/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var collection = await _collectionService.GetByIdAsync(id);

            if (collection == null)
                return NotFound("Collection not found.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            if (collection.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            return Ok(collection);
        }

        // GET: api/Collection/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMyCollections()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var collections = await _collectionService.GetByUserIdAsync(userId);

            return Ok(collections);
        }

        // GET: api/Collection/search?name=MyTools
        [HttpGet("search")]
        public async Task<IActionResult> Search(string name)
        {
            var collections = await _collectionService.SearchAsync(name);

            return Ok(collections);
        }

        // GET: api/Collection/paged?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1,int pageSize = 10)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            if (pageSize > 100)
                pageSize = 100;

            var collections =await _collectionService.GetPagedAsync(pageNumber,pageSize);

            return Ok(collections);
        }

        // POST: api/Collection
        [HttpPost]
        public async Task<IActionResult> Create(CollectionCreateDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var collection = new Collection
            {
                UserId = userId,
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _collectionService.CreateAsync(collection);

            _logger.LogInformation("Collection {CollectionId} created by User {UserId}.",result.Id,userId);

            return CreatedAtAction(nameof(GetById),new { id = result.Id },result);
        }

        // PUT: api/Collection/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,CollectionUpdateDto dto)
        {
            var existingCollection = await _collectionService.GetByIdAsync(id);

            if (existingCollection == null)
                return NotFound("Collection not found.");

            var userIdClaim =User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            if (existingCollection.UserId != userId)
                return Forbid();

            var collection = new Collection
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var result = await _collectionService.UpdateAsync(id, collection);

            if (!result)
                return NotFound("Collection not found.");

            var updatedCollection =await _collectionService.GetByIdAsync(id);

            _logger.LogInformation("Collection {CollectionId} updated by User {UserId}.",id,userId);

            return Ok(updatedCollection);
        }

        // DELETE: api/Collection/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var collection = await _collectionService.GetByIdAsync(id);

            if (collection == null)
                return NotFound("Collection not found.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            if (collection.UserId != userId)
                return Forbid();

            var result = await _collectionService.DeleteAsync(id);

            if (!result)
                return NotFound("Collection not found.");

            _logger.LogInformation("Collection {CollectionId} deleted by User {UserId}.",id,userId);

            return Ok("Collection deleted successfully.");
        }
    }
}
