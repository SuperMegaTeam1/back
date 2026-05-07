namespace Backend.Application.Models;

public sealed record NotificationResult(
    Guid Id,
    string SenderName,
    string SenderLastName,
    string? SenderFatherName,
    string Title,
    string Body,
    bool IsRead,
    DateTime CreatedAt
);