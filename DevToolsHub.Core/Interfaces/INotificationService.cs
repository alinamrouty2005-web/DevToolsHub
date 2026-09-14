using DevToolsHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> GetAllAsync();

        Task<Notification?> GetByIdAsync(int id);

        Task<List<Notification>> GetByUserIdAsync(int userId);

        Task<List<Notification>> GetUnreadByUserIdAsync(int userId);

        Task<Notification> CreateAsync(Notification notification);

        Task<bool> UpdateAsync(int id,Notification notification);

        Task<bool> DeleteAsync(int id);
    }
}
