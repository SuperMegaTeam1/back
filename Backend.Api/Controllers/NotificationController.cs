using System.Security.Claims;
using Backend.Api.Contracts.Notification;
using Backend.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;
[Route("api")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    
    public  NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    [HttpGet("notification")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
    public async Task<ActionResult<IReadOnlyCollection<NotificationResponse>>> GetNotifications()
    {
        var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(user, out var userId))
        {
            return Unauthorized();
        }
        
        var response = await _notificationService.GetNotificationsAsync(userId);

        var result = response 
            .Select(notification => new NotificationResponse(
                notification.Id,
                notification.SenderName,
                notification.SenderLastName,
                notification.SenderFatherName,
                notification.Title,
                notification.Body,
                notification.IsRead,
                notification.CreatedAt.ToString("O")
            ))
            .ToList();
        return Ok(result);
    }

    [HttpPost("teacher-message")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
    public async Task<IActionResult> PostNotifications(TeacherMessageRequest messageRequest)
    {
        var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(user, out var userId))
        {
            return Unauthorized();
        }
        
        var response = await _notificationService.CreateNotificationsAsync(
            userId,
            messageRequest.groupId, 
            messageRequest.Title, 
            messageRequest.Body);

        if (response == null)
        {
            return BadRequest("Не удалось отправить сообщение.");
        }    
            
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPatch("read-all")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
    public async Task<IActionResult> PatchNotifications()
    {
        var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
    
        if (!Guid.TryParse(user, out var userId))
        {
            return  Unauthorized();
        }

        await _notificationService.PatchStatusNotificationsAsync(userId);
        
        return NoContent();
    }
}