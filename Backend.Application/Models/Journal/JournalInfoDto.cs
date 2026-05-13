namespace Backend.Application.Models.Journal;

public sealed record JournalInfoDto(
    DateTime lessonsStartDate,
    int studentGrade);