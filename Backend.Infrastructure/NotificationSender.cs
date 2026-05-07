using Backend.Application;
using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Infrastructure;

public class NotificationSender : INotificationSender
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IHubContext<NotificationHub> _hubContext;
    
    public NotificationSender(
        INotificationRepository notificationRepository,
        IHubContext<NotificationHub> hubContext)
    {
        _notificationRepository = notificationRepository;
        _hubContext = hubContext;
    }
    
    public async Task SendNotificationAsync(Notification notification)
    {
        await _hubContext.Clients.User(notification.ReceiverId.ToString())
            .SendAsync("ReceiveNotification", new 
            { 
                id = notification.Id,
                title = notification.Title, 
                body = notification.Body,
                createdAt = notification.CreatedAt
            });
    }
}