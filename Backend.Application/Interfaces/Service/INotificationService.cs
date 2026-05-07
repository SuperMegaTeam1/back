using Backend.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Application.Interfaces;

public interface INotificationService
{
    Task<IReadOnlyCollection<NotificationResult>> GetNotificationsAsync(Guid userId);
    Task<TeacherMessageResponse?> CreateNotificationsAsync(Guid senderId, Guid groupId, string title, string message);
    Task PatchStatusNotificationsAsync(Guid receiverId);
}