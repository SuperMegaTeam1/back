using Backend.Application.Interfaces;
using Backend.Application.Models.Journal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    public class GradesController : ControllerBase
    {
        public readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [Authorize(Roles = "Teacher")]
        [HttpPatch("/grades/{gradeId}")]
        public async Task<IActionResult> UpdateGrade(
            Guid gradeId,
            [FromBody] UpdateGradeRequest request)
        {
            var result = await _gradeService.UpdateGrade(gradeId, request.Grade);

            return Ok(new
            {
                id = gradeId,
                studentId = result.StudentId,
                lessonId = result.LessonId,
                grade = result.Grade,
                updatedAt = DateTime.UtcNow
            });
        }
    }
}
