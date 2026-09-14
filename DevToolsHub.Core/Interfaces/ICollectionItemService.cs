using DevToolsHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using DevToolsHub.Core.DTOs;


namespace DevToolsHub.Core.Interfaces
{
    public interface ICollectionItemService
    {
        Task<List<CollectionItem>> GetAllAsync();

        Task<CollectionItem?> GetByIdAsync(int collectionId,int toolId);

        Task<List<CollectionItem>> GetByCollectionIdAsync(int collectionId);

        Task<List<CollectionItem>> GetByToolIdAsync(int toolId);

        Task<CollectionItem> CreateAsync(CollectionItem collectionItem);

        Task<bool> DeleteAsync(int collectionId,int toolId);
    }
}
