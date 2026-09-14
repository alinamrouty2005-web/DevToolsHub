using System;
using System.Collections.Generic;
using System.Text;
using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Data;
using Microsoft.EntityFrameworkCore;


namespace DevToolsHub.DataAccess.Repositories
{
    public class ProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetAllAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Project>> GetByUserIdAsync(int userId)
        {
            return await _context.Projects.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<List<Project>> SearchAsync(string name)
        {
            return await _context.Projects.Where(p => p.Name.Contains(name)).ToListAsync();
        }

        public async Task<List<Project>> GetPagedAsync(int pageNumber,int pageSize)
        {
            return await _context.Projects.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}

    

