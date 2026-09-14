using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace DevToolsHub.DataAccess.Repositories
{
    public class CollectionItemRepository
    {
        private readonly AppDbContext _context;

        public CollectionItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CollectionItem>> GetAllAsync()
        {
            return await _context.CollectionItems.ToListAsync();
        }

        public async Task<CollectionItem?> GetByIdAsync(int collectionId,int toolId)
        {
            return await _context.CollectionItems.FirstOrDefaultAsync(ci => ci.CollectionId == collectionId && ci.ToolId == toolId);
        }

        public async Task<List<CollectionItem>> GetByCollectionIdAsync(int collectionId)
        {
            return await _context.CollectionItems.Where(ci => ci.CollectionId == collectionId).ToListAsync();
        }

        public async Task<List<CollectionItem>> GetByToolIdAsync(int toolId)
        {
            return await _context.CollectionItems.Where(ci => ci.ToolId == toolId).ToListAsync();
        }

        public async Task AddAsync(CollectionItem collectionItem)
        {
            await _context.CollectionItems.AddAsync(collectionItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CollectionItem collectionItem)
        {
            _context.CollectionItems.Remove(collectionItem);
            await _context.SaveChangesAsync();
        }
    }
}
