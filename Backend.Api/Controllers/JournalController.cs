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
    }
}
