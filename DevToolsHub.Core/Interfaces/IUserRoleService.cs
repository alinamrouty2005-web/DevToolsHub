using DevToolsHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Interfaces
{
    public interface IUserRoleService
    {
        Task<List<UserRole>> GetAllAsync();

        Task<UserRole?> GetByIdAsync(int userId, int roleId);

        Task<UserRole> CreateAsync(UserRole userRole);

        Task<bool> DeleteAsync(int userId, int roleId);
    }
}
