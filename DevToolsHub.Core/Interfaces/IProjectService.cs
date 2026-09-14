using DevToolsHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Interfaces
{
    public interface IProjectService
    {
        Task<List<Project>> GetAllAsync();

        Task<Project?> GetByIdAsync(int id);

        Task<List<Project>> GetByUserIdAsync(int userId);

        Task<List<Project>> SearchAsync(string name);

        Task<List<Project>> GetPagedAsync(int pageNumber,int pageSize);

        Task<Project> CreateAsync(Project project);

        Task<bool> UpdateAsync(int id, Project project);

        Task<bool> DeleteAsync(int id);
    }
}
