using System.ComponentModel.DataAnnotations;

namespace Backend.Api.Contracts.Notification;

public sealed record TeacherMessageResponse(
    string groupName,
    string Title,
    string Body
    );