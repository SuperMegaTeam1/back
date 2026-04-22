using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Api.Contracts.Auth;
using Backend.Application.Interfaces;

namespace Backend.Api.Controllers
{
    [Route("auth")]
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
                return Unauthorized();
            }

            var response = new AuthResponse(
                Token: result.Token,
                User: new AuthUserResponse(
                    Id: result.User.Id,
                    RoleId: result.User.RoleId,
                    FirstName: result.User.FirstName,
                    LastName: result.User.LastName,
                    FatherName: result.User.FatherName,
                    Email: result.User.Email,
                    RoleName: result.User.RoleName
                )
            );

            return Ok(response);
        }
    }
}
