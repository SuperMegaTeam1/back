using Backend.Api.Contracts.Auth;
using Backend.Application.Interfaces;
using Backend.Application.Models.Journal;
using Backend.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Api.Controllers
{
    public class JournalController : ControllerBase
    {
        private readonly IJournalService _journalService;

        public JournalController(IJournalService journalService)
        {
            _journalService = journalService;
        }

        [Authorize(Roles = "Teacher")]
        [HttpPut("/lessons/{lessonId}/journal")]
        public async Task<IActionResult> UpdateJournal(
            Guid lessonId,
            [FromBody] UpdateJournalRequest request)
        {
            var result = await _journalService.UpdateJournal(lessonId, request);
            return Ok(result);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("journal/{subjectId}/{groupId}")]
        public async Task<IActionResult> GetJournal(Guid subjectId, Guid groupId)
        {
            var teacherIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(teacherIdValue, out var teacherId))
                return Unauthorized();
            var journal = await _journalService.GetJournalAsync(subjectId, groupId, teacherId);
            return Ok(journal);
        }
    }
}
