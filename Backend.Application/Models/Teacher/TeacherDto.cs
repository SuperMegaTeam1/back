namespace Backend.Application.Models;

public sealed record TeacherDto(
    Guid teacherId,
    Guid parentUserId,
    string firstName,
    string lastName,
    string fatherName);