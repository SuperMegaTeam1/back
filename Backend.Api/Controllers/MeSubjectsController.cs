using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Service;
using Backend.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Api.Controllers
{
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
    [ApiController]
    public class MeSubjectsController : ControllerBase
    {
        private readonly IStudentsSubjectsService _studentsSubjectsService;

        public MeSubjectsController(IStudentsSubjectsService studentsSubjects)
        {
            _studentsSubjectsService = studentsSubjects;
        }

        [HttpGet("students/me/subjects")]
        [Authorize]
        public async Task<IActionResult> GetMySubjects()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
                return Unauthorized();

            var subjects = await _studentsSubjectsService.GetSubjectsForStudentAsync(userId);

            return Ok(new { items = subjects });
        }
    }
}

