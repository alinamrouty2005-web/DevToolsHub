using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Infrastructure.Services
{
    public class CollectionItemService : ICollectionItemService
    {
        private readonly CollectionItemRepository _collectionItemRepository;

        public CollectionItemService(CollectionItemRepository collectionItemRepository)
        {
            _collectionItemRepository = collectionItemRepository;
        }

        public async Task<List<CollectionItem>> GetAllAsync()
        {
            return await _collectionItemRepository.GetAllAsync();
        }

        public async Task<CollectionItem?> GetByIdAsync(int collectionId,int toolId)
        {
            return await _collectionItemRepository.GetByIdAsync(collectionId, toolId);
        }

        public async Task<List<CollectionItem>> GetByCollectionIdAsync(int collectionId)
        {
            return await _collectionItemRepository.GetByCollectionIdAsync(collectionId);
        }

        public async Task<List<CollectionItem>> GetByToolIdAsync(int toolId)
        {
            return await _collectionItemRepository.GetByToolIdAsync(toolId);
        }

        public async Task<CollectionItem> CreateAsync(CollectionItem collectionItem)
        {
            await _collectionItemRepository.AddAsync(collectionItem);

            return collectionItem;
        }

        public async Task<bool> DeleteAsync(int collectionId,int toolId)
        {
            var collectionItem =await _collectionItemRepository.GetByIdAsync(collectionId, toolId);

            if (collectionItem == null)
                return false;

            await _collectionItemRepository.DeleteAsync(collectionItem);

            return true;
        }
    }
}
