using DevToolsHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Interfaces
{
    public interface IToolHistoryService
    {
        Task<List<ToolHistory>> GetAllAsync();

        Task<ToolHistory?> GetByIdAsync(int id);

        Task<List<ToolHistory>> GetByUserIdAsync(int userId);

        Task<List<ToolHistory>> GetByToolIdAsync(int toolId);

        Task<List<ToolHistory>> GetByProjectIdAsync(int projectId);

        Task<List<ToolHistory>> GetPagedAsync(int pageNumber,int pageSize);

        Task<ToolHistory> CreateAsync(ToolHistory toolHistory);

        Task<bool> UpdateAsync(int id, ToolHistory toolHistory);

        Task<bool> DeleteAsync(int id);
    }
}
