using Backend.Domain.Entities;

namespace Backend.Application.Interfaces;

public interface INotificationSender
{
    Task SendNotificationAsync(Notification notification);
}