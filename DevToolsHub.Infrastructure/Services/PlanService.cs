using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Infrastructure.Services
{
    public class PlanService : IPlanService
    {
        private readonly PlanRepository _planRepository;

        public PlanService(PlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<List<Plan>> GetAllAsync()
        {
            return await _planRepository.GetAllAsync();
        }

        public async Task<Plan?> GetByIdAsync(int id)
        {
            return await _planRepository.GetByIdAsync(id);
        }

        public async Task<Plan?> GetByNameAsync(string name)
        {
            return await _planRepository.GetByNameAsync(name);
        }

        public async Task<List<Plan>> GetActiveAsync()
        {
            return await _planRepository.GetActiveAsync();
        }

        public async Task<List<Plan>> SearchAsync(string name)
        {
            return await _planRepository.SearchAsync(name);
        }

        public async Task<Plan> CreateAsync(Plan plan)
        {
            await _planRepository.AddAsync(plan);

            return plan;
        }

        public async Task<bool> UpdateAsync(int id,Plan plan)
        {
            var existingPlan =await _planRepository.GetByIdAsync(id);

            if (existingPlan == null)
                return false;

            existingPlan.Name = plan.Name;
            existingPlan.Price = plan.Price;
            existingPlan.DurationInDays = plan.DurationInDays;
            existingPlan.IsActive = plan.IsActive;

            await _planRepository.UpdateAsync(existingPlan);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var plan =await _planRepository.GetByIdAsync(id);

            if (plan == null)
                return false;

            await _planRepository.DeleteAsync(plan);

            return true;
        }
    }
}
