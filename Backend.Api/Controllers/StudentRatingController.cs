using Backend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Api.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentRatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public StudentRatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet("me/rating")]
        [Authorize]
        public async Task<IActionResult> GetMyRating([FromQuery] Guid? subjectId)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
                return Unauthorized();

            var result = await _ratingService.GetMyRatingAsync(userId, subjectId);

            return Ok(result);
        }
    }
}
