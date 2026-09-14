using DevToolsHub.Core.DTOs;
using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DevToolsHub.DataAccess.Data;
using DevToolsHub.Infrastructure.Services;





namespace DevToolsHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CollectionItemController : ControllerBase
    {
        private readonly ICollectionItemService _collectionItemService;
        private readonly ICollectionService _collectionService;
        private readonly ILogger<CollectionItemController> _logger;

        public CollectionItemController(
            ICollectionItemService collectionItemService,
            ICollectionService collectionService,
            ILogger<CollectionItemController> logger)
        {
            _collectionItemService = collectionItemService;
            _collectionService = collectionService;
            _logger = logger;
        }

        // GET: api/CollectionItem
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var items =await _collectionItemService.GetAllAsync();

            return Ok(items);
        }

        // GET: api/CollectionItem/1/5
        [HttpGet("{collectionId}/{toolId}")]
        public async Task<IActionResult> GetById(
            int collectionId,
            int toolId)
        {
            var item = await _collectionItemService.GetByIdAsync(collectionId,toolId);

            if (item == null)
                return NotFound("Collection item not found.");

            return Ok(item);
        }

        // GET: api/CollectionItem/collection/1
        [HttpGet("collection/{collectionId}")]
        public async Task<IActionResult> GetByCollectionId(int collectionId)
        {
            var collection =await _collectionService.GetByIdAsync(collectionId);

            if (collection == null)
                return NotFound("Collection not found.");

            var items = await _collectionItemService.GetByCollectionIdAsync(collectionId);

            return Ok(items);
        }

        // GET: api/CollectionItem/tool/5
        [HttpGet("tool/{toolId}")]
        public async Task<IActionResult> GetByToolId(int toolId)
        {
            var items =await _collectionItemService.GetByToolIdAsync(toolId);

            return Ok(items);
        }

        // POST: api/CollectionItem
        [HttpPost]
        public async Task<IActionResult> Create(CollectionItemCreateDto dto)
        {
            var existingItem =await _collectionItemService.GetByIdAsync(dto.CollectionId,dto.ToolId);

            if (existingItem != null)
                return BadRequest("Tool already exists in this collection.");

            var collection = await _collectionService.GetByIdAsync(dto.CollectionId);

            if (collection == null)
                return NotFound("Collection not found.");

            var collectionItem = new CollectionItem
            {
                CollectionId = dto.CollectionId,
                ToolId = dto.ToolId
            };

            var result =await _collectionItemService.CreateAsync(collectionItem);

            _logger.LogInformation("Tool {ToolId} added to Collection {CollectionId}.",result.ToolId,result.CollectionId);

            return Ok(result);
        }

        // DELETE: api/CollectionItem/1/5
        [HttpDelete("{collectionId}/{toolId}")]
        public async Task<IActionResult> Delete(int collectionId,int toolId)
        {
            var item =await _collectionItemService.GetByIdAsync(collectionId,toolId);

            if (item == null)
                return NotFound("Collection item not found.");

            var collection =await _collectionService.GetByIdAsync(collectionId);

            if (collection == null)
                return NotFound("Collection not found.");

            var result = await _collectionItemService.DeleteAsync(collectionId,toolId);

            if (!result)
                return NotFound("Collection item not found.");

            _logger.LogInformation("Tool {ToolId} removed from Collection {CollectionId}.",toolId,collectionId);

            return Ok("Tool removed from collection successfully.");
        }
    }
}
