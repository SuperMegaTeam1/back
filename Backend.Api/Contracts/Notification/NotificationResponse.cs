namespace Backend.Api.Contracts.Notification;

public sealed record NotificationResponse(
    Guid Id,
    string SenderName,
    string SenderLastName,
    string? SenderFatherName, 
    string Title,
    string MessageBody,
    bool isRead,
    string CreatedAt
);