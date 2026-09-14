using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Infrastructure.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly SubscriptionRepository _subscriptionRepository;

        public SubscriptionService(SubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<List<Subscription>> GetAllAsync()
        {
            return await _subscriptionRepository.GetAllAsync();
        }

        public async Task<Subscription?> GetByIdAsync(int id)
        {
            return await _subscriptionRepository.GetByIdAsync(id);
        }

        public async Task<Subscription?> GetByUserIdAsync(int userId)
        {
            return await _subscriptionRepository.GetByUserIdAsync(userId);
        }

        public async Task<List<Subscription>> GetByPlanIdAsync(int planId)
        {
            return await _subscriptionRepository.GetByPlanIdAsync(planId);
        }

        public async Task<List<Subscription>> GetActiveAsync()
        {
            return await _subscriptionRepository.GetActiveAsync();
        }

        public async Task<Subscription> CreateAsync(Subscription subscription)
        {
            await _subscriptionRepository.AddAsync(subscription);

            return subscription;
        }

        public async Task<bool> UpdateAsync(int id,Subscription subscription)
        {
            var existingSubscription =await _subscriptionRepository.GetByIdAsync(id);

            if (existingSubscription == null)
                return false;

            existingSubscription.PlanId =subscription.PlanId;

            existingSubscription.StartDate =subscription.StartDate;

            existingSubscription.EndDate =subscription.EndDate;

            existingSubscription.IsActive =subscription.IsActive;

            await _subscriptionRepository.UpdateAsync(existingSubscription);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(id);

            if (subscription == null)
                return false;

            await _subscriptionRepository.DeleteAsync(subscription);

            return true;
        }
    }
}
