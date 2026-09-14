using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace DevToolsHub.DataAccess.Repositories
{
    public class ToolHistoryRepository
    {
        private readonly AppDbContext _context;

        public ToolHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ToolHistory>> GetAllAsync()
        {
            return await _context.ToolHistories.ToListAsync();
        }

        public async Task<ToolHistory?> GetByIdAsync(int id)
        {
            return await _context.ToolHistories.FirstOrDefaultAsync(th => th.Id == id);
        }

        public async Task<List<ToolHistory>> GetByUserIdAsync(int userId)
        {
            return await _context.ToolHistories.Where(th => th.UserId == userId).ToListAsync();
        }

        public async Task<List<ToolHistory>> GetByToolIdAsync(int toolId)
        {
            return await _context.ToolHistories.Where(th => th.ToolId == toolId).ToListAsync();
        }

        public async Task<List<ToolHistory>> GetByProjectIdAsync(int projectId)
        {
            return await _context.ToolHistories.Where(th => th.ProjectId == projectId).ToListAsync();
        }

        public async Task<List<ToolHistory>> GetPagedAsync(int pageNumber,int pageSize)
        {
            return await _context.ToolHistories
                .OrderByDescending(th => th.ExecutedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddAsync(ToolHistory toolHistory)
        {
            await _context.ToolHistories.AddAsync(toolHistory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ToolHistory toolHistory)
        {
            _context.ToolHistories.Update(toolHistory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ToolHistory toolHistory)
        {
            _context.ToolHistories.Remove(toolHistory);
            await _context.SaveChangesAsync();
        }
    }
}
