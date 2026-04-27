using Backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("today")]
        public async Task<IActionResult> Schedule([FromQuery] DateOnly? date) 
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var response = await _sheduleService.GetTodayScheduleAsync(userId, date);

            return Ok(response);
        }
    }
}
