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
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IPlanService _planService;
        private readonly ILogger<SubscriptionController> _logger;

        public SubscriptionController(
            ISubscriptionService subscriptionService,
            IPlanService planService,
            ILogger<SubscriptionController> logger)
        {
            _subscriptionService = subscriptionService;
            _planService = planService;
            _logger = logger;
        }


        // GET: api/Subscription
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var subscriptions = await _subscriptionService.GetAllAsync();

            var result = new List<SubscriptionResponseDto>();

            foreach (var subscription in subscriptions)
            {
                var dto = await MapToResponseDto(subscription);

                result.Add(dto);
            }

            return Ok(result);
        }


        // GET: api/Subscription/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMySubscription()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var subscription = await _subscriptionService.GetByUserIdAsync(userId.Value);

            if (subscription == null)
                return NotFound("You do not have a subscription.");

            var result = await MapToResponseDto(subscription);

            return Ok(result);
        }


        // GET: api/Subscription/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var subscription =await _subscriptionService.GetByIdAsync(id);

            if (subscription == null)
                return NotFound("Subscription not found.");

            var result = await MapToResponseDto(subscription);

            return Ok(result);
        }


        // POST: api/Subscription
        [HttpPost]
        public async Task<IActionResult> Create(SubscriptionCreateDto dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var plan =await _planService.GetByIdAsync(dto.PlanId);

            if (plan == null)
                return NotFound("Plan not found.");

            if (!plan.IsActive)
                return BadRequest("This plan is not active.");

            var existingSubscription =await _subscriptionService.GetByUserIdAsync(userId.Value);

            if (existingSubscription != null)
                return BadRequest("User already has a subscription.");

            var startDate = DateTime.UtcNow;

            var subscription = new Subscription
            {
                UserId = userId.Value,
                PlanId = plan.Id,
                StartDate = startDate,
                EndDate = startDate.AddDays(
                    plan.DurationInDays),
                IsActive = true
            };

            var createdSubscription =await _subscriptionService.CreateAsync(subscription);

            _logger.LogInformation("Subscription created. SubscriptionId: {SubscriptionId}, UserId: {UserId}, PlanId: {PlanId}",
                createdSubscription.Id,
                userId.Value,
                plan.Id);

            var result =await MapToResponseDto(createdSubscription);

            return CreatedAtAction(nameof(GetMySubscription),null,result);
        }


        // PUT: api/Subscription/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id,SubscriptionUpdateDto dto)
        {
            var subscription =await _subscriptionService.GetByIdAsync(id);

            if (subscription == null)
                return NotFound("Subscription not found.");

            var plan =await _planService.GetByIdAsync(dto.PlanId);

            if (plan == null)
                return NotFound("Plan not found.");

            subscription.PlanId = dto.PlanId;
            subscription.IsActive = dto.IsActive;

            var updated = await _subscriptionService.UpdateAsync(id,subscription);

            if (!updated)
                return NotFound("Subscription not found.");

            _logger.LogInformation("Subscription updated. SubscriptionId: {SubscriptionId}",id);

            return Ok(new
            {
                success = true,
                message = "Subscription updated successfully."
            });
        }


        // DELETE: api/Subscription/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var subscription =await _subscriptionService.GetByIdAsync(id);

            if (subscription == null)
                return NotFound("Subscription not found.");

            var deleted =await _subscriptionService.DeleteAsync(id);

            if (!deleted)
                return NotFound("Subscription not found.");

            _logger.LogInformation("Subscription deleted. SubscriptionId: {SubscriptionId}",id);

            return Ok(new
            {
                success = true,
                message = "Subscription deleted successfully."
            });
        }


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


        private async Task<SubscriptionResponseDto>
            MapToResponseDto(Subscription subscription)
        {
            var plan =await _planService.GetByIdAsync(subscription.PlanId);

            return new SubscriptionResponseDto
            {
                Id = subscription.Id,
                UserId = subscription.UserId,
                PlanId = subscription.PlanId,
                PlanName = plan?.Name ?? string.Empty,
                Price = plan?.Price ?? 0,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                IsActive = subscription.IsActive
            };
        }
    }
}
