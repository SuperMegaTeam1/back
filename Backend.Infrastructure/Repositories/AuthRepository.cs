using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Interfaces;
using Backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Backend.Application.Models;

namespace Backend.Infrastructure.Repository
{
    public sealed class AuthRepository: IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AuthUser?> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return null;
            }

            return new AuthUser(
                Id: user.Id,
                FirstName: user.FirstName ?? string.Empty,
                LastName: user.LastName ?? string.Empty,
                FatherName: user.FatherName,
                Email: user.Email ?? string.Empty);
        }

        public async Task<bool> CheckPasswordAsync(Guid userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }   
    }
}
