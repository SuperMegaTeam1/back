namespace Backend.Application.Models;

public sealed record StudentsResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string? FatherName,
    string? Email);