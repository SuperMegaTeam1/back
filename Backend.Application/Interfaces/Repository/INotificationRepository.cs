using Backend.Application.Models;
using Backend.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Application.Interfaces;

public interface INotificationRepository
{
    Task<IReadOnlyCollection<NotificationResult>> GetNotificationsAsync(Guid userId);
    Task CreateNotificationsAsync(List<Notification> notifications);
    Task<IReadOnlyCollection<Notification>?> GetNotificationsStudentAsync(Guid receiverId);
    Task PatchNotificationsAsync(IReadOnlyCollection<Notification> notifications);
}