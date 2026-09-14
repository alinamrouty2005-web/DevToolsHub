using DevToolsHub.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DevToolsHub.Core.Models;


namespace DevToolsHub.DataAccess.Repositories
{
    public class CollectionRepository
    {
        private readonly AppDbContext _context;

        public CollectionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Collection>> GetAllAsync()
        {
            return await _context.Collections.ToListAsync();
        }

        public async Task<Collection?> GetByIdAsync(int id)
        {
            return await _context.Collections.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Collection>> GetByUserIdAsync(int userId)
        {
            return await _context.Collections.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<List<Collection>> SearchAsync(string name)
        {
            return await _context.Collections.Where(c => c.Name.Contains(name)).ToListAsync();
        }

        public async Task<List<Collection>> GetPagedAsync(int pageNumber,int pageSize)
        {
            return await _context.Collections
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddAsync(Collection collection)
        {
            await _context.Collections.AddAsync(collection);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Collection collection)
        {
            _context.Collections.Update(collection);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Collection collection)
        {
            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();
        }
    }
}
