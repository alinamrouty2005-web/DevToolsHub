using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Infrastructure.Services
{
    public class ToolHistoryService : IToolHistoryService
    {
        private readonly ToolHistoryRepository _toolHistoryRepository;

        public ToolHistoryService(ToolHistoryRepository toolHistoryRepository)
        {
            _toolHistoryRepository = toolHistoryRepository;
        }

        public async Task<List<ToolHistory>> GetAllAsync()
        {
            return await _toolHistoryRepository.GetAllAsync();
        }

        public async Task<ToolHistory?> GetByIdAsync(int id)
        {
            return await _toolHistoryRepository.GetByIdAsync(id);
        }

        public async Task<List<ToolHistory>> GetByUserIdAsync(int userId)
        {
            return await _toolHistoryRepository.GetByUserIdAsync(userId);
        }

        public async Task<List<ToolHistory>> GetByToolIdAsync(int toolId)
        {
            return await _toolHistoryRepository.GetByToolIdAsync(toolId);
        }

        public async Task<List<ToolHistory>> GetByProjectIdAsync(int projectId)
        {
            return await _toolHistoryRepository.GetByProjectIdAsync(projectId);
        }

        public async Task<List<ToolHistory>> GetPagedAsync(int pageNumber,int pageSize)
        {
            return await _toolHistoryRepository.GetPagedAsync(pageNumber, pageSize);
        }

        public async Task<ToolHistory> CreateAsync(ToolHistory toolHistory)
        {
            await _toolHistoryRepository.AddAsync(toolHistory);

            return toolHistory;
        }

        public async Task<bool> UpdateAsync(int id, ToolHistory toolHistory)
        {
            var existingHistory = await _toolHistoryRepository.GetByIdAsync(id);

            if (existingHistory == null)
                return false;

            existingHistory.ToolId = toolHistory.ToolId;
            existingHistory.ProjectId = toolHistory.ProjectId;
            existingHistory.InputData = toolHistory.InputData;
            existingHistory.OutputData = toolHistory.OutputData;

            await _toolHistoryRepository.UpdateAsync(existingHistory);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var toolHistory = await _toolHistoryRepository.GetByIdAsync(id);

            if (toolHistory == null)
                return false;

            await _toolHistoryRepository.DeleteAsync(toolHistory);

            return true;
        }
    }
}
