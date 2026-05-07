using Backend.Application.Interfaces;
using Backend.Application.Models;
using Backend.Domain.Entities;

namespace Backend.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IGroupRepository _groupRepository;
    
    public NotificationService(
        INotificationRepository notificationRepository,
        IStudentRepository studentRepository,
        IGroupRepository groupRepository)
    {
        _notificationRepository = notificationRepository;
        _groupRepository = groupRepository;
        _studentRepository = studentRepository;
    }

    public async Task<IReadOnlyCollection<NotificationResult>> GetNotificationsAsync(Guid userId)
    {
        var notifications = await _notificationRepository.GetNotificationsAsync(userId);

        if (notifications == null || notifications.Count == 0)
        {
            return Array.Empty<NotificationResult>();
        }
        
        return notifications;
    }

    public async Task<TeacherMessageResponse?> CreateNotificationsAsync(
        Guid senderId,
        Guid groupId,
        string title, 
        string message)
    {
        var group = await _groupRepository.GetGroupAsync(groupId);

        if (group == null)
        {
            return null;
        }
        
        var students = await _studentRepository.GetByGroupIdAsync(groupId);

        if (students == null || students.Count == 0)
        {
            return null;
        }

        var notification = students
            .Select(student => new Notification
                {
                    Id = Guid.NewGuid(),
                    SenderId = senderId,
                    ReceiverId = student.ParentUserId,
                    Title = title,
                    Body = message,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                }
            )
            .ToList();
        
        await _notificationRepository.CreateNotificationsAsync(notification);
        
        return new TeacherMessageResponse(
            group.Name,
            title, 
            message);
    }

    public async Task PatchStatusNotificationsAsync(Guid receiverId)
    {
        var notificationsForStudent = await _notificationRepository.GetNotificationsStudentAsync(receiverId);
        
        if (notificationsForStudent == null || notificationsForStudent.Count == 0)
        {
            return;
        }

        foreach (var notification in notificationsForStudent)
        {
            notification.IsRead = true;
        }
        
        await _notificationRepository.PatchNotificationsAsync(notificationsForStudent);
    }
}