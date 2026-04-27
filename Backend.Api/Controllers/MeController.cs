using Backend.Api.Contracts.Auth;
using Backend.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly IAuthService _authService;

        public MeController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("me")]
        public async Task<ActionResult<AuthUserResponse>> Me()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var user = await _authService.GetByIdAsync(userId);

            if (user is null)
            {
                return Unauthorized();
            }

            var response = new AuthUserResponse(
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

            return Ok(response);
        }
    }
}
