using Backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Models;
using Microsoft.AspNetCore.Authentication;
using System.Runtime.InteropServices;


namespace Backend.Application.Services
{
    public sealed class AuthService: IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;

        public AuthService(
            IAuthRepository authRepository,
            ITokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
        }
        public async Task<AuthResult?> LoginAsync(string email, string password)
        {
            var user = await _authRepository.FindByEmailAsync(email);

            if (user is null)
            {
                return null;
            }

            var passwordValid = await _authRepository.CheckPasswordAsync(user.Id, password);

            if (!passwordValid)
            {
                return null;
            }

            var token = _tokenService.CreateToken(user);

            return new AuthResult(
                Token: token,
                new AuthUserResult(
                    Id: user.Id,
                    FirstName: user.FirstName,
                    LastName: user.LastName,
                    FatherName: user.FatherName,
                    Email: user.Email));
        }
    }
}
