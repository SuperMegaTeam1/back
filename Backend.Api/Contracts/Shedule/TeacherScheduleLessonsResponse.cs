namespace Backend.Api.Contracts.Shedule
{
    public sealed record TeacherScheduleLessonsResponse(
        Guid LessonsId,
        Guid SubjectId,
        string SubjectName,
        string? Cabinet,
        string? Type,
        string StartsAt,
        string EndsAt);
}
