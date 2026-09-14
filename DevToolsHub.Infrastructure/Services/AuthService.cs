using DevToolsHub.Core.DTOs;
using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;



namespace DevToolsHub.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;
        private readonly UserRoleRepository _userRoleRepository;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(
            UserRepository userRepository,
            RoleRepository roleRepository,
            UserRoleRepository userRoleRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

            if (existingUser != null)
                return false;

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                IsActive = true
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _userRepository.AddAsync(user);

            var userRole = await _roleRepository.GetByNameAsync("User");

            if (userRole != null)
            {
                var newUserRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = userRole.Id
                };

                await _userRoleRepository.AddAsync(newUserRole);
            }

            return true;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
                return null;

            if (!user.IsActive)
                return null;

            var passwordResult =_passwordHasher.VerifyHashedPassword(user,user.PasswordHash,dto.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
                return null;

            var userRole =(await _userRoleRepository.GetAllAsync()).FirstOrDefault(ur => ur.UserId == user.Id);

            string? roleName = null;

            if (userRole != null)
            {
                var role = await _roleRepository.GetByIdAsync(userRole.RoleId);

                roleName = role?.Name;
            }

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.FullName),

                new Claim(ClaimTypes.Email,user.Email)
            };

            if (!string.IsNullOrEmpty(roleName))
            {
                claims.Add(new Claim(ClaimTypes.Role,roleName));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials =new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var durationInMinutes = int.Parse(_configuration["Jwt:DurationInMinutes"]!);

            var expiration = DateTime.UtcNow.AddMinutes(durationInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new LoginResponseDto
            {
                Token =new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
