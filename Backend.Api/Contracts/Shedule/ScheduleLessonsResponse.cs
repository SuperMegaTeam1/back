namespace Backend.Api.Contracts.Shedule
{
    public sealed record ScheduleLessonsResponse(
        Guid LessonsId,
        Guid SubjectId,
        string SubjectName,
        Guid TeacherId,
        string TeacherFirstName,
        string TeacherLastName,
        string? TeacherFatherName,
        string? Cabinet,
        string? Type,
        string StartsAt,
        string EndsAt);
}
