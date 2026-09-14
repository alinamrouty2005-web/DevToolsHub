using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleRepository _roleRepository;

        public RoleService(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _roleRepository.GetAllAsync();
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _roleRepository.GetByNameAsync(name);
        }

        public async Task<Role> CreateAsync(Role role)
        {
            await _roleRepository.AddAsync(role);

            return role;
        }

        public async Task<bool> UpdateAsync(int id, Role role)
        {
            var existingRole = await _roleRepository.GetByIdAsync(id);

            if (existingRole == null)
                return false;

            existingRole.Name = role.Name;
            existingRole.Description = role.Description;

            await _roleRepository.UpdateAsync(existingRole);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);

            if (role == null)
                return false;

            await _roleRepository.DeleteAsync(role);

            return true;
        }
    }
}
