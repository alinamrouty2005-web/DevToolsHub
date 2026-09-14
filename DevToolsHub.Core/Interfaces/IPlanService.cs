using DevToolsHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Interfaces
{
    public interface IPlanService
    {
        Task<List<Plan>> GetAllAsync();

        Task<Plan?> GetByIdAsync(int id);

        Task<Plan?> GetByNameAsync(string name);

        Task<List<Plan>> GetActiveAsync();

        Task<List<Plan>> SearchAsync(string name);

        Task<Plan> CreateAsync(Plan plan);

        Task<bool> UpdateAsync(int id, Plan plan);

        Task<bool> DeleteAsync(int id);
    }
}
