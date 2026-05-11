namespace Backend.Application.Models;

public sealed record StudentsDto(
    Guid Id,
    string FirstName,
    string LastName,
    string FatherName,
    string Email);