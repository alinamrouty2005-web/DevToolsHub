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
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: api/Notification
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notifications = await _notificationService.GetAllAsync();

            return Ok(notifications);
        }

        // GET: api/Notification/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var notification = await _notificationService.GetByIdAsync(id);

            if (notification == null)
                return NotFound("Notification not found.");

            return Ok(notification);
        }

        // GET: api/Notification/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var notifications = await _notificationService.GetByUserIdAsync(userId);

            return Ok(notifications);
        }

        // GET: api/Notification/unread
        [HttpGet("unread")]
        public async Task<IActionResult> GetUnread()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var notifications = await _notificationService.GetUnreadByUserIdAsync(userId);

            return Ok(notifications);
        }

        // POST: api/Notification
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(NotificationCreateDto dto)
        {
            var notification = new Notification
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Message = dto.Message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _notificationService.CreateAsync(notification);

            return CreatedAtAction(nameof(GetById),new { id = result.Id },result);
        }

        // PUT: api/Notification/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,NotificationUpdateDto dto)
        {
            var notification = new Notification
            {
                Title = dto.Title,
                Message = dto.Message,
                IsRead = dto.IsRead
            };

            var result = await _notificationService.UpdateAsync(id, notification);

            if (!result)
                return NotFound("Notification not found.");

            var updatedNotification = await _notificationService.GetByIdAsync(id);

            return Ok(updatedNotification);
        }

        // DELETE: api/Notification/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _notificationService.DeleteAsync(id);

            if (!result)
                return NotFound("Notification not found.");

            return Ok("Notification deleted successfully.");
        }
    }
}

