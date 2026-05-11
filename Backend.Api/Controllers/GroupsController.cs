using System.Security.Claims;
using Backend.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[Route("api")]
[ApiController]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;
    
    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
    [HttpGet("groups")]
    public async Task<IActionResult> GetGroups()
    {
        var userValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userValue, out var userId))
        {
            return Unauthorized();
        }

        var result = await _groupService.GetTeacherGroupsAsync(userId);
        
        return Ok(result);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
    [HttpGet("{groupdId}/students")]
    public async Task<IActionResult> GetStudentsFromGroup(Guid groupdId)
    {
        var userValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userValue, out var userId))
        {
            return  Unauthorized();
        }

        var result = await _groupService.GetStudentsByGroupIdAsync(groupdId);

        if (result.Count == 0)
        {
            return NotFound();
        }
            
        return Ok(result);
    }
}