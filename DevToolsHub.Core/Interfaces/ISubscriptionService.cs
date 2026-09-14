using DevToolsHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Interfaces
{
    public interface ISubscriptionService
    {
        Task<List<Subscription>> GetAllAsync();

        Task<Subscription?> GetByIdAsync(int id);

        Task<Subscription?> GetByUserIdAsync(int userId);

        Task<List<Subscription>> GetByPlanIdAsync(int planId);

        Task<List<Subscription>> GetActiveAsync();

        Task<Subscription> CreateAsync(Subscription subscription);

        Task<bool> UpdateAsync(int id,Subscription subscription);

        Task<bool> DeleteAsync(int id);
    }
}
