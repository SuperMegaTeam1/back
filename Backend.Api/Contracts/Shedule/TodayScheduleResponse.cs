namespace Backend.Api.Contracts.Shedule
{
    public sealed record TodayScheduleresponse(
        string Date,
        string DayName,
        int WeekNumber,
        int LessonsCount,
        IReadOnlyCollection<ScheduleLessonsResponse> Items);

}
