using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Infrastructure.Services
{
    public class ToolService : IToolService
    {
        private readonly ToolRepository _toolRepository;

        public ToolService(ToolRepository toolRepository)
        {
            _toolRepository = toolRepository;
        }

        public async Task<List<Tool>> GetAllAsync()
        {
            return await _toolRepository.GetAllAsync();
        }

        public async Task<Tool?> GetByIdAsync(int id)
        {
            return await _toolRepository.GetByIdAsync(id);
        }

        public async Task<Tool?> GetByNameAsync(string name)
        {
            return await _toolRepository.GetByNameAsync(name);
        }

        public async Task<List<Tool>> SearchAsync(string name)
        {
            return await _toolRepository.SearchAsync(name);
        }

        public async Task<List<Tool>> GetByCategoryAsync(string category)
        {
            return await _toolRepository.GetByCategoryAsync(category);
        }

        public async Task<List<Tool>> GetActiveAsync()
        {
            return await _toolRepository.GetActiveAsync();
        }

        public async Task<List<Tool>> GetPagedAsync(int pageNumber,int pageSize,string? search,string? category,string? sortBy,bool sortDescending)
        {
            return await _toolRepository.GetPagedAsync(pageNumber,pageSize,search,category,sortBy,sortDescending);
        }

        public async Task<Tool> CreateAsync(Tool tool)
        {
            await _toolRepository.AddAsync(tool);

            return tool;
        }

        public async Task<bool> UpdateAsync(int id,Tool tool)
        {
            var existingTool =await _toolRepository.GetByIdAsync(id);

            if (existingTool == null)
                return false;

            existingTool.Name = tool.Name;
            existingTool.Description = tool.Description;
            existingTool.Category = tool.Category;
            existingTool.IsActive = tool.IsActive;

            await _toolRepository.UpdateAsync(existingTool);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tool = await _toolRepository.GetByIdAsync(id);

            if (tool == null)
                return false;

            await _toolRepository.DeleteAsync(tool);

            return true;
        }
    }
}
