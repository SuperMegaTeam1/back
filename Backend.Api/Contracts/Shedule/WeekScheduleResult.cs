namespace Backend.Api.Contracts.Shedule;

public record WeekScheduleResult<T>(
    string DateStart,
    string DateEnd,
    IReadOnlyCollection<TodayScheduleResponse<T>> Items);