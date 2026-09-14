using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace DevToolsHub.DataAccess.Repositories
{
    public class ToolRepository
    {
        private readonly AppDbContext _context;

        public ToolRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tool>> GetAllAsync()
        {
            return await _context.Tools.ToListAsync();
        }

        public async Task<Tool?> GetByIdAsync(int id)
        {
            return await _context.Tools.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tool?> GetByNameAsync(string name)
        {
            return await _context.Tools.FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<List<Tool>> SearchAsync(string name)
        {
            return await _context.Tools.Where(t => t.Name.Contains(name)).ToListAsync();
        }

        public async Task<List<Tool>> GetByCategoryAsync(string category)
        {
            return await _context.Tools.Where(t => t.Category == category).ToListAsync();
        }

        public async Task<List<Tool>> GetActiveAsync()
        {
            return await _context.Tools.Where(t => t.IsActive).ToListAsync();
        }

        public async Task<List<Tool>> GetPagedAsync(int pageNumber,int pageSize,string? search,string? category,string? sortBy,bool sortDescending)
        {
            var tools = await _context.Tools.ToListAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                tools = tools.Where(t =>
                        t.Name.Contains(search) || (t.Description != null && t.Description.Contains(search))).ToList();
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                tools = tools.Where(t => t.Category == category).ToList();
            }

            if (sortBy == "name")
            {
                if (sortDescending)
                    tools = tools.OrderByDescending(t => t.Name).ToList();
                else
                    tools = tools.OrderBy(t => t.Name).ToList();
            }
            else if (sortBy == "createdAt")
            {
                if (sortDescending)
                    tools = tools.OrderByDescending(t => t.CreatedAt).ToList();
                else
                    tools = tools.OrderBy(t => t.CreatedAt).ToList();
            }

            tools = tools.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return tools;
        }

        public async Task AddAsync(Tool tool)
        {
            await _context.Tools.AddAsync(tool);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tool tool)
        {
            _context.Tools.Update(tool);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tool tool)
        {
            _context.Tools.Remove(tool);
            await _context.SaveChangesAsync();
        }
    }
}
