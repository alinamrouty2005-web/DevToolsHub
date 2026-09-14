using DevToolsHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();

        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByEmailAsync(string email);

        Task<User> CreateAsync(User user);

        Task<bool> UpdateAsync(int id, User user);

        Task<bool> DeleteAsync(int id);
    }
}
