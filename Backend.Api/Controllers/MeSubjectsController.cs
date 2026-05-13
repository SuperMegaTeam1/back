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
    [ApiController]
    public class MeSubjectsController : ControllerBase
    {
        private readonly IStudentsSubjectsService _studentsSubjectsService;
        private readonly ITeachersSubjectsService _teachersSubjectsService;
        private readonly ISubjectService _subjectService;

        public MeSubjectsController(
            IStudentsSubjectsService studentsSubjects, 
            ITeachersSubjectsService teachersSubjectsService,
            ISubjectService subjectService)
        {
            _studentsSubjectsService = studentsSubjects;
            _teachersSubjectsService = teachersSubjectsService;
            _subjectService = subjectService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
        [HttpGet("students/me/subjects")]
        [Authorize]
        public async Task<IActionResult> GetStudentsSubjects()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
                return Unauthorized();

            var subjects = await _studentsSubjectsService.GetSubjectsForStudentAsync(userId);

            return Ok(new { items = subjects });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        [HttpGet("teachers/me/subjects")]
        [Authorize]
        public async Task<IActionResult> GetTeachersSubjects()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
                return Unauthorized();

            Console.WriteLine(userId);

            var subjects = await _teachersSubjectsService.GetSubjectsForTeacherAsync(userId);

            return Ok(new { items = subjects });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
        [HttpGet("subjects/{subjectId}")]
        public async Task<IActionResult> GetSubject(Guid subjectId)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }
            
            var subjectInfo = await _subjectService.GetSubjectInfoAsync(userId, subjectId);

            if (subjectInfo == null)
            {
                return NotFound();
            }
            
            return Ok(subjectInfo);
        }
    }
}

