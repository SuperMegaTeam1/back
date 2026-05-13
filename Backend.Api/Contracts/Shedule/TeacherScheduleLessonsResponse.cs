namespace Backend.Api.Contracts.Shedule
{
    public sealed record TeacherScheduleLessonsResponse(
        Guid LessonsId,
        Guid SubjectId,
        string SubjectName,
        string? Cabinet,
        string? Type,
        List<StudyGroupResponse>? StudyGroups,
        string StartsAt,
        string EndsAt);

    public sealed record StudyGroupResponse(Guid Id, string Name);
}
