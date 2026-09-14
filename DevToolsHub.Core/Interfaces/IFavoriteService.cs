using DevToolsHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Interfaces
{
    public interface IFavoriteService
    {
        Task<List<Favorite>> GetAllAsync();

        Task<Favorite?> GetByIdAsync(int id);

        Task<Favorite?> GetByUserAndToolAsync(int userId,int toolId);

        Task<List<Favorite>> GetByUserIdAsync(int userId);

        Task<Favorite> CreateAsync(Favorite favorite);

        Task<bool> DeleteAsync(int id);
    }
}
