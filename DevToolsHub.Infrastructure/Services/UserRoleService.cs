using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Infrastructure.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserRoleRepository _userRoleRepository;

        public UserRoleService(UserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<List<UserRole>> GetAllAsync()
        {
            return await _userRoleRepository.GetAllAsync();
        }

        public async Task<UserRole?> GetByIdAsync(int userId,int roleId)
        {
            return await _userRoleRepository.GetByIdAsync(userId, roleId);
        }

        public async Task<UserRole> CreateAsync(UserRole userRole)
        {
            await _userRoleRepository.AddAsync(userRole);

            return userRole;
        }

        public async Task<bool> DeleteAsync(int userId,int roleId)
        {
            var userRole = await _userRoleRepository.GetByIdAsync(userId, roleId);

            if (userRole == null)
                return false;

            await _userRoleRepository.DeleteAsync(userRole);

            return true;
        }
    }
}
