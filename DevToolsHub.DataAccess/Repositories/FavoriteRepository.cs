using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace DevToolsHub.DataAccess.Repositories
{
    public class FavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Favorite>> GetAllAsync()
        {
            return await _context.Favorites.ToListAsync();
        }

        public async Task<Favorite?> GetByIdAsync(int id)
        {
            return await _context.Favorites.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Favorite?> GetByUserAndToolAsync(int userId,int toolId)
        {
            return await _context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.ToolId == toolId);
        }

        public async Task<List<Favorite>> GetByUserIdAsync(int userId)
        {
            return await _context.Favorites.Where(f => f.UserId == userId).ToListAsync();
        }

        public async Task AddAsync(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Favorite favorite)
        {
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }
    }
}
