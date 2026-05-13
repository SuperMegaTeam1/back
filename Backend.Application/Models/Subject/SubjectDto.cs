namespace Backend.Application.Models.Subject;

public sealed record SubjectDto(
    Guid Id,
    string Name,
    Guid TeacherId);