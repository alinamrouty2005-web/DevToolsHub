using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace DevToolsHub.DataAccess.Repositories
{
    public class SubscriptionRepository
    {
        private readonly AppDbContext _context;

        public SubscriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Subscription>> GetAllAsync()
        {
            return await _context.Subscriptions.ToListAsync();
        }

        public async Task<Subscription?> GetByIdAsync(int id)
        {
            return await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Subscription?> GetByUserIdAsync(int userId)
        {
            return await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<List<Subscription>> GetByPlanIdAsync(int planId)
        {
            return await _context.Subscriptions.Where(s => s.PlanId == planId).ToListAsync();
        }

        public async Task<List<Subscription>> GetActiveAsync()
        {
            return await _context.Subscriptions.Where(s => s.IsActive).ToListAsync();
        }

        public async Task AddAsync(Subscription subscription)
        {
            await _context.Subscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Subscription subscription)
        {
            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
        }
    }
}
