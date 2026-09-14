using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Infrastructure.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly FavoriteRepository _favoriteRepository;

        public FavoriteService(FavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<List<Favorite>> GetAllAsync()
        {
            return await _favoriteRepository.GetAllAsync();
        }

        public async Task<Favorite?> GetByIdAsync(int id)
        {
            return await _favoriteRepository.GetByIdAsync(id);
        }

        public async Task<Favorite?> GetByUserAndToolAsync(int userId,int toolId)
        {
            return await _favoriteRepository.GetByUserAndToolAsync(userId, toolId);
        }

        public async Task<List<Favorite>> GetByUserIdAsync(int userId)
        {
            return await _favoriteRepository.GetByUserIdAsync(userId);
        }

        public async Task<Favorite> CreateAsync(Favorite favorite)
        {
            await _favoriteRepository.AddAsync(favorite);

            return favorite;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var favorite =await _favoriteRepository.GetByIdAsync(id);

            if (favorite == null)
                return false;

            await _favoriteRepository.DeleteAsync(favorite);

            return true;
        }
    }
}
