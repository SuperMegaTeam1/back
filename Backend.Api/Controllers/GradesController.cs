using System.Security.Claims;
using Backend.Application.Interfaces;
using Backend.Application.Models.Journal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        public readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [Authorize(Roles = "Teacher")]
        [HttpPatch("grades/{gradeId}")]
        public async Task<IActionResult> UpdateGrade(
            Guid gradeId,
            [FromBody] UpdateGradeRequest request)
        {
            var userValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (!Guid.TryParse(userValue, out var userId))
            {
                return Unauthorized();
            }
            
            var result = await _gradeService.UpdateGrade(
                 userId,
                 gradeId,
                 request.Grade,
                 request.Attended);
            
            return Ok(new
            {
                id = gradeId,
                studentId = result.StudentId,
                lessonId = result.LessonId,
                grade = result.Grade,
                attended = result.Attended,
                updatedAt = DateTime.UtcNow
            });
        }
    }
}
