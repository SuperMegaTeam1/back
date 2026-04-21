using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Api.Contracts.Auth;

namespace Backend.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var response = new AuthResponse(
                Token: "asd",
                User: new AuthUserResponse(
                    Id: new Guid(),
                    RoleId: new Guid(),
                    FirstName: "Igor",
                    LastName: "Suchov",
                    FatherName: "Sergeevich",
                    Email: request.Email,
                    UserType: 1
                    )
                );

            var password = request.Password;    

            return Ok(response);
        }
    }
}
