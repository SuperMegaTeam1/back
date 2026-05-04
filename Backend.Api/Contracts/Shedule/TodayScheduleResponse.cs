namespace Backend.Api.Contracts.Shedule
{
    public sealed record TodayScheduleResponse<T>(
        string Date,
        string DayName,
        int WeekNumber,
        int LessonsCount,
        IReadOnlyCollection<T> Items);
}
