using Backend.Application.Interfaces;
using Backend.Application.Models;

namespace Backend.Application.Services
{
    public sealed class AuthService : IAuthService
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
                User: MapToResult(user));
        }

        public async Task<AuthUserResult?> GetByIdAsync(Guid userId)
        {
            var user = await _authRepository.FindByIdAsync(userId);

            if (user is null)
            {
                return null;
            }

            return MapToResult(user);
        }

        private static AuthUserResult MapToResult(AuthUser user)
        {
            return new AuthUserResult(
                Id: user.Id,
                RoleName: user.RoleName,
                FirstName: user.FirstName,
                LastName: user.LastName,
                FatherName: user.FatherName,
                Email: user.Email,
                StudentId: user.StudentId,
                TeacherId: user.TeacherId,
                GroupId: user.GroupId,
                GroupName: user.GroupName);
        }
    }
}
