using DevToolsHub.Core.DTOs;
using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevToolsHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly IPlanService _planService;
        private readonly ILogger<PlanController> _logger;

        public PlanController(IPlanService planService,ILogger<PlanController> logger)
        {
            _planService = planService;
            _logger = logger;
        }


        // GET: api/Plan
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var plans = await _planService.GetAllAsync();

            var result = plans.Select(MapToResponseDto).ToList();

            return Ok(result);
        }


        // GET: api/Plan/active
        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActive()
        {
            var plans = await _planService.GetActiveAsync();

            var result = plans.Select(MapToResponseDto).ToList();

            return Ok(result);
        }


        // GET: api/Plan/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var plan =await _planService.GetByIdAsync(id);

            if (plan == null)
                return NotFound("Plan not found.");

            return Ok(MapToResponseDto(plan));
        }


        // GET: api/Plan/search?name=Pro
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required.");

            var plans =await _planService.SearchAsync(name);

            var result = plans.Select(MapToResponseDto).ToList();

            return Ok(result);
        }


        // POST: api/Plan
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PlanCreateDto dto)
        {
            var existingPlan = await _planService.GetByNameAsync(dto.Name);

            if (existingPlan != null)
                return BadRequest("A plan with this name already exists.");

            var plan = new Plan
            {
                Name = dto.Name,
                Price = dto.Price,
                DurationInDays = dto.DurationInDays,
                IsActive = dto.IsActive
            };

            var createdPlan =await _planService.CreateAsync(plan);

            _logger.LogInformation("Plan created. PlanId: {PlanId}, PlanName: {PlanName}",createdPlan.Id,createdPlan.Name);

            var result = MapToResponseDto(createdPlan);

            return CreatedAtAction(nameof(GetById),new { id = result.Id },result);
        }


        // PUT: api/Plan/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id,PlanUpdateDto dto)
        {
            var existingPlan =await _planService.GetByIdAsync(id);

            if (existingPlan == null)
                return NotFound("Plan not found.");

            var planWithSameName =await _planService.GetByNameAsync(dto.Name);

            if (planWithSameName != null && planWithSameName.Id != id)
            {
                return BadRequest("A plan with this name already exists.");
            }

            existingPlan.Name = dto.Name;
            existingPlan.Price = dto.Price;
            existingPlan.DurationInDays = dto.DurationInDays;
            existingPlan.IsActive = dto.IsActive;

            var updated = await _planService.UpdateAsync(id,existingPlan);

            if (!updated)
                return NotFound("Plan not found.");

            _logger.LogInformation("Plan updated. PlanId: {PlanId}",id);

            return Ok(new
            {
                success = true,
                message = "Plan updated successfully."
            });
        }


        // DELETE: api/Plan/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var plan = await _planService.GetByIdAsync(id);

            if (plan == null)
                return NotFound("Plan not found.");

            var deleted = await _planService.DeleteAsync(id);

            if (!deleted)
                return NotFound("Plan not found.");

            _logger.LogInformation("Plan deleted. PlanId: {PlanId}",id);

            return Ok(new
            {
                success = true,
                message = "Plan deleted successfully."
            });
        }


        private PlanResponseDto MapToResponseDto(Plan plan)
        {
            return new PlanResponseDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                DurationInDays = plan.DurationInDays,
                IsActive = plan.IsActive
            };
        }
    }
}
