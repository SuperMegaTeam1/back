using Backend.Application.Models.Journal;

namespace Backend.Application.Models.Subject;

public sealed record SubjectResponse(
    Guid Id,
    string Name,
    Guid TeacherId,
    string TeacherName,
    string TeacherLastName,
    string? TeacherFatherName,
    List<JournalInfoDto> JournalInfos);