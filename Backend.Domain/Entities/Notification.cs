using Backend.Domain.Interfaces;

namespace Backend.Domain.Entities;

public class Notification : IEntityWithId, IEntityWithChangeInfo
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}