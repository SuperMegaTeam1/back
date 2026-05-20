using Backend.Application.Interfaces;
using Backend.Application.Models;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _dbContext;
    
    public NotificationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<NotificationResult>> GetNotificationsAsync(Guid userId)
    {
        var notifications = await _dbContext.Notifications
            .Where(x => userId == x.ReceiverId)
            .OrderByDescending(x => x.CreatedAt)
            .Join(
                _dbContext.Users, 
                x => x.SenderId,
                user => user.Id,
                (x, user) => new NotificationResult(
                    x.Id,
                    user.FirstName,
                    user.LastName,
                    user.FatherName,
                    x.Title,
                    x.Body,
                    x.IsRead,
                    x.CreatedAt))
            .ToListAsync();
        
        return notifications;
    }

    public async Task CreateNotificationsAsync(List<Notification> notifications)
    {
        await _dbContext.Notifications.AddRangeAsync(notifications);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task CreateNotificationAsync(Notification notifications)
    {
        await _dbContext.Notifications.AddAsync(notifications);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<IReadOnlyCollection<Notification>?> GetNotificationsStudentAsync(Guid receiverId)
    {
        return await _dbContext.Notifications
            .Where(x => 
                x.ReceiverId == receiverId &&
                !x.IsRead)
            .ToListAsync();
    }
    
    public async Task PatchNotificationsAsync(IReadOnlyCollection<Notification> notifications)
    {
        _dbContext.Notifications.UpdateRange(notifications);
        await _dbContext.SaveChangesAsync();
    }
}