namespace Backend.Api.Contracts.Auth;

public sealed record AuthTeacherResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string? FatherName,
    string Email,
    Guid? TeacherId);