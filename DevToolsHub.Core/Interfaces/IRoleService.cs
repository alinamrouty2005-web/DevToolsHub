using DevToolsHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllAsync();

        Task<Role?> GetByIdAsync(int id);

        Task<Role?> GetByNameAsync(string name);

        Task<Role> CreateAsync(Role role);

        Task<bool> UpdateAsync(int id, Role role);

        Task<bool> DeleteAsync(int id);
    }
}
