using System;
using System.Collections.Generic;
using System.Text;
using DevToolsHub.Core.Models;



namespace DevToolsHub.Core.Interfaces
{
    public interface ICollectionService
    {
        Task<List<Collection>> GetAllAsync();

        Task<Collection?> GetByIdAsync(int id);

        Task<List<Collection>> GetByUserIdAsync(int userId);

        Task<List<Collection>> SearchAsync(string name);

        Task<List<Collection>> GetPagedAsync(int pageNumber,int pageSize);

        Task<Collection> CreateAsync(Collection collection);

        Task<bool> UpdateAsync(int id, Collection collection);

        Task<bool> DeleteAsync(int id);
    }
}
