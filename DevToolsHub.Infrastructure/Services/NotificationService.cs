using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationRepository _notificationRepository;

        public NotificationService(NotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<List<Notification>> GetAllAsync()
        {
            return await _notificationRepository.GetAllAsync();
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _notificationRepository.GetByIdAsync(id);
        }

        public async Task<List<Notification>> GetByUserIdAsync(int userId)
        {
            return await _notificationRepository.GetByUserIdAsync(userId);
        }

        public async Task<List<Notification>> GetUnreadByUserIdAsync(int userId)
        {
            return await _notificationRepository.GetUnreadByUserIdAsync(userId);
        }

        public async Task<Notification> CreateAsync(Notification notification)
        {
            await _notificationRepository.AddAsync(notification);

            return notification;
        }

        public async Task<bool> UpdateAsync(int id,Notification notification)
        {
            var existingNotification = await _notificationRepository.GetByIdAsync(id);

            if (existingNotification == null)
                return false;

            existingNotification.Title = notification.Title;
            existingNotification.Message = notification.Message;
            existingNotification.IsRead = notification.IsRead;

            await _notificationRepository.UpdateAsync(existingNotification);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);

            if (notification == null)
                return false;

            await _notificationRepository.DeleteAsync(notification);

            return true;
        }
    }
}
