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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
        [HttpGet("student/me")]
        public async Task<ActionResult<AuthStudentResponse>> StudentMe()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var student = await _authService.GetByIdAsync(userId);

            if (student is null ||  student.StudentId is null)
            {
                return Unauthorized();
            }

            var response = new AuthStudentResponse(
                Id: student.Id,
                RoleName: student.RoleName,
                FirstName: student.FirstName,
                LastName: student.LastName,
                FatherName: student.FatherName,
                Email: student.Email,
                StudentId: student.StudentId,
                GroupId: student.GroupId,
                GroupName: student.GroupName);

            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        [HttpGet("teacher/me")]
        public async Task<ActionResult<AuthTeacherResponse>> TeacherMe()
        {
            var userValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userValue, out var userId))
            {
                return Unauthorized();
            }
            
            var teacher = await _authService.GetByIdAsync(userId);

            if (teacher is null || teacher.TeacherId is null)
            {
                Console.WriteLine("A{SD{AS{DPAS{DPASOP{DSAOOPDIASOIJDJKASJDKLASKDOASLKFJUEHIUQHUIHWQUIUHEUQWHJEUQWHJEKJQWHEKJHQWKJEHQWKJEH");
                return Unauthorized();
            }

            var response = new AuthTeacherResponse(
                Id: teacher.Id,
                FirstName: teacher.FirstName,
                LastName: teacher.LastName,
                FatherName: teacher.FatherName,
                Email: teacher.Email,
                TeacherId: teacher.TeacherId);

            return Ok(response);
        }
    }
}
