using System.ComponentModel.DataAnnotations;

namespace Backend.Api.Contracts.Notification;

public sealed record TeacherMessageRequest(
    [Required]
    Guid groupId,
    string Title,
    [Required]
    string Body);