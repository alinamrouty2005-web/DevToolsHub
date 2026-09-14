using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace DevToolsHub.DataAccess.Repositories
{
    public class PlanRepository
    {
        private readonly AppDbContext _context;

        public PlanRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Plan>> GetAllAsync()
        {
            return await _context.Plans.ToListAsync();
        }

        public async Task<Plan?> GetByIdAsync(int id)
        {
            return await _context.Plans.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Plan?> GetByNameAsync(string name)
        {
            return await _context.Plans.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<List<Plan>> GetActiveAsync()
        {
            return await _context.Plans.Where(p => p.IsActive).ToListAsync();
        }

        public async Task<List<Plan>> SearchAsync(string name)
        {
            return await _context.Plans.Where(p => p.Name.Contains(name)).ToListAsync();
        }

        public async Task AddAsync(Plan plan)
        {
            await _context.Plans.AddAsync(plan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Plan plan)
        {
            _context.Plans.Update(plan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Plan plan)
        {
            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();
        }
    }
}
