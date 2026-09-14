using DevToolsHub.Core.Interfaces;
using DevToolsHub.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using DevToolsHub.Core.Models;

namespace DevToolsHub.Infrastructure.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly CollectionRepository _collectionRepository;

        public CollectionService(CollectionRepository collectionRepository)
        {
            _collectionRepository = collectionRepository;
        }

        public async Task<List<Collection>> GetAllAsync()
        {
            return await _collectionRepository.GetAllAsync();
        }

        public async Task<Collection?> GetByIdAsync(int id)
        {
            return await _collectionRepository.GetByIdAsync(id);
        }

        public async Task<List<Collection>> GetByUserIdAsync(int userId)
        {
            return await _collectionRepository.GetByUserIdAsync(userId);
        }

        public async Task<List<Collection>> SearchAsync(string name)
        {
            return await _collectionRepository.SearchAsync(name);
        }

        public async Task<List<Collection>> GetPagedAsync(int pageNumber,int pageSize)
        {
            return await _collectionRepository.GetPagedAsync(pageNumber, pageSize);
        }

        public async Task<Collection> CreateAsync(Collection collection)
        {
            await _collectionRepository.AddAsync(collection);

            return collection;
        }

        public async Task<bool> UpdateAsync(int id,Collection collection)
        {
            var existingCollection = await _collectionRepository.GetByIdAsync(id);

            if (existingCollection == null)
                return false;

            existingCollection.Name = collection.Name;
            existingCollection.Description = collection.Description;

            await _collectionRepository.UpdateAsync(existingCollection);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var collection = await _collectionRepository.GetByIdAsync(id);

            if (collection == null)
                return false;

            await _collectionRepository.DeleteAsync(collection);

            return true;
        }
    }
}
