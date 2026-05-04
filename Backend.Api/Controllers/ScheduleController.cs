using Backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Api.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _sheduleService;

        public ScheduleController(IScheduleService sheduleService)
        {
            _sheduleService = sheduleService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
        [HttpGet("student/today")]
        public async Task<IActionResult> StudentSchedule([FromQuery] DateOnly? date)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var response = await _sheduleService.GetTodayScheduleAsync(userId, date);

            return Ok(response);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        [HttpGet("teacher/today")]
        public async Task<IActionResult> TeacherSchedule([FromQuery] DateOnly? date)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var response = await _sheduleService.GetTodayScheduleAsync(userId, date);

            return Ok(response);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
        [HttpGet("student/week")]
        public async Task<IActionResult> StudentScheduleWeek([FromQuery] DateOnly? date)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var response = await _sheduleService.GetWeekScheduleAsync(userId, date);

            return Ok(response);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        [HttpGet("teacher/week")]
        public async Task<IActionResult> TeacherScheduleWeek([FromQuery] DateOnly? date)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var response = await _sheduleService.GetWeekScheduleAsync(userId, date);

            return Ok(response);
        }
    }
}
