using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace DevToolsHub.DataAccess.Repositories
{
    public class ApiRequestRepository
    {
        private readonly AppDbContext _context;

        public ApiRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApiRequest>> GetAllAsync()
        {
            return await _context.ApiRequests.ToListAsync();
        }

        public async Task<ApiRequest?> GetByIdAsync(int id)
        {
            return await _context.ApiRequests.FirstOrDefaultAsync(ar => ar.Id == id);
        }

        public async Task<List<ApiRequest>> GetByUserIdAsync(int userId)
        {
            return await _context.ApiRequests.Where(ar => ar.UserId == userId).ToListAsync();
        }

        public async Task<List<ApiRequest>> GetByProjectIdAsync(int projectId)
        {
            return await _context.ApiRequests.Where(ar => ar.ProjectId == projectId).ToListAsync();
        }

        public async Task<List<ApiRequest>> GetByMethodAsync(string method)
        {
            return await _context.ApiRequests.Where(ar => ar.Method == method).ToListAsync();
        }

        public async Task<List<ApiRequest>> GetPagedAsync(int pageNumber,int pageSize)
        {
            return await _context.ApiRequests.OrderByDescending(ar => ar.CreatedAt).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task AddAsync(ApiRequest apiRequest)
        {
            await _context.ApiRequests.AddAsync(apiRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ApiRequest apiRequest)
        {
            _context.ApiRequests.Update(apiRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ApiRequest apiRequest)
        {
            _context.ApiRequests.Remove(apiRequest);
            await _context.SaveChangesAsync();
        }
    }
}
