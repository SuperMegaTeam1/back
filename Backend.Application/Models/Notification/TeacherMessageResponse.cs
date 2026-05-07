namespace Backend.Application.Models;

public sealed record TeacherMessageResponse(
    string groupName,
    string title,
    string message);