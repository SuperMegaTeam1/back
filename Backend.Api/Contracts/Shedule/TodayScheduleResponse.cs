namespace Backend.Api.Contracts.Shedule
{
    public sealed record TodayStudentScheduleresponse(
        string Date,
        string DayName,
        int WeekNumber,
        int LessonsCount,
        IReadOnlyCollection<ScheduleLessonsResponse> Items);

}
