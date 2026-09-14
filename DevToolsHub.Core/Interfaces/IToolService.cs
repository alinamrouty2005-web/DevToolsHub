using DevToolsHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Interfaces
{
    public interface IToolService
    {
        Task<List<Tool>> GetAllAsync();

        Task<Tool?> GetByIdAsync(int id);

        Task<Tool?> GetByNameAsync(string name);

        Task<List<Tool>> SearchAsync(string name);

        Task<List<Tool>> GetByCategoryAsync(string category);

        Task<List<Tool>> GetActiveAsync();

        Task<List<Tool>> GetPagedAsync(int pageNumber,int pageSize,string? search,string? category,string? sortBy,bool sortDescending);

        Task<Tool> CreateAsync(Tool tool);

        Task<bool> UpdateAsync(int id, Tool tool);

        Task<bool> DeleteAsync(int id);
    }
}
