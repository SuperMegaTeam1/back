using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Api.Contracts.Auth;
using Backend.Application.Interfaces;

namespace Backend.Api.Controllers
{
    // Возможно надо будет убрать из него лишние данные. Оставить только токен
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService; 

        public AuthController(IAuthService authService)
        {
            _authService = authService; 
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.Email, request.Password);

            if (result == null)
            {
                return Unauthorized(); // NotFound() 401 вроде должнав прольщховаться проверить
            }

            var response = new AuthResponse(Token: result.Token);

            return Ok(response);
        }
    }
}
