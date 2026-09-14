using System;
using System.Collections.Generic;
using System.Text;
using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;


namespace DevToolsHub.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User> CreateAsync(User user)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);

            await _userRepository.AddAsync(user);

            return user;

        }

        public async Task<bool> UpdateAsync(int id, User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);

            if (existingUser == null)
                return false;

            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            existingUser.IsActive = user.IsActive;

            await _userRepository.UpdateAsync(existingUser);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            await _userRepository.DeleteAsync(user);

            return true;
        }
    }
}
