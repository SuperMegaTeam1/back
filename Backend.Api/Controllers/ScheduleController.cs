using Backend.Api.Contracts.Shedule;
using Backend.Application.Interfaces;
using Backend.Application.Models.Shedule;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Api.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [Authorize(Roles = "Student")]
        [HttpGet("student/today")]
        public async Task<ActionResult<TodayScheduleResponse<StudentScheduleLessonsResponse>>> GetStudentToday([FromQuery] DateOnly? date)
        {
            var userId = GetUserId();
            var result = await _scheduleService.GetTodayScheduleAsync(userId, date);

            if (result == null) return NotFound();

            return Ok(MapToTodayResponse(result, MapToStudentLesson));
        }

        [Authorize(Roles = "Student")]
        [HttpGet("student/week")]
        public async Task<ActionResult<WeekScheduleResult<StudentScheduleLessonsResponse>>> GetStudentWeek([FromQuery] DateOnly? date)
        {
            var userId = GetUserId();
            var result = await _scheduleService.GetWeekScheduleAsync(userId, date);

            if (result == null) return NotFound();

            var response = new WeekScheduleResult<StudentScheduleLessonsResponse>(
                DateStart: result.DateStart,
                DateEnd: result.DateEnd,
                Items: result.Items.Select(d => MapToTodayResponse(d, MapToStudentLesson)).ToList()
            );

            return Ok(response);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("teacher/today")]
        public async Task<ActionResult<TodayScheduleResponse<TeacherScheduleLessonsResponse>>> GetTeacherToday([FromQuery] DateOnly? date)
        {
            var userId = GetUserId();
            var result = await _scheduleService.GetTodayScheduleAsync(userId, date);

            if (result == null) return NotFound();

            return Ok(MapToTodayResponse(result, MapToTeacherLesson));
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("teacher/week")]
        public async Task<ActionResult<WeekScheduleResult<TeacherScheduleLessonsResponse>>> GetTeacherWeek([FromQuery] DateOnly? date)
        {
            var userId = GetUserId();
            var result = await _scheduleService.GetWeekScheduleAsync(userId, date);

            if (result == null) return NotFound();

            var response = new WeekScheduleResult<TeacherScheduleLessonsResponse>(
                DateStart: result.DateStart,
                DateEnd: result.DateEnd,
                Items: result.Items.Select(d => MapToTodayResponse(d, MapToTeacherLesson)).ToList()
            );

            return Ok(response);
        }

        private TodayScheduleResponse<T> MapToTodayResponse<T>(
            TodayScheduleResult source, 
            Func<ScheduleLessonsResult, T> lessonMapper)
        {
            return new TodayScheduleResponse<T>(
                Date: source.Date,
                DayName: source.DayName,
                WeekNumber: source.WeekNumber ?? 0,
                LessonsCount: source.Items?.Count ?? 0,
                Items: source.Items?.Select(lessonMapper).ToList() ?? new List<T>()
            );
        }

        private StudentScheduleLessonsResponse MapToStudentLesson(ScheduleLessonsResult item) => new(
            LessonsId: item.LessonsId,
            SubjectId: item.SubjectId,
            SubjectName: item.SubjectName,
            TeacherId: item.TeacherId ?? Guid.Empty,
            TeacherFirstName: item.TeacherFirstName ?? "",
            TeacherLastName: item.TeacherLastName ?? "",
            TeacherFatherName: item.TeacherFatherName,
            Cabinet: item.Cabinet,
            Type: item.Type,
            StartsAt: item.StartsAt,
            EndsAt: item.EndsAt
        );

        private TeacherScheduleLessonsResponse MapToTeacherLesson(ScheduleLessonsResult item) => new(
            LessonsId: item.LessonsId,
            SubjectId: item.SubjectId,
            SubjectName: item.SubjectName,
            Cabinet: item.Cabinet,
            Type: item.Type,
            StartsAt: item.StartsAt,
            EndsAt: item.EndsAt
        );

        private Guid GetUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
        }
    }
}