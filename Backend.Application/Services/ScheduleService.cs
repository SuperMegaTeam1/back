using Backend.Application.Interfaces;
using Backend.Application.Models.Shedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Services
{
    public sealed class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
    
        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<WeekScheduleResult> GetWeekScheduleAsync(Guid userId, DateOnly? date)
        {
            var actualDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);
            var (monday, saturday) = StartEndDay(actualDate);

            var days = await _scheduleRepository.GetScheduleAsync(userId, monday, saturday);

            return new WeekScheduleResult(
                DateStart: monday.ToString("yyyy-MM-dd"),
                DateEnd: saturday.ToString("yyyy-MM-dd"),
                Items: days
            );
        }

        private (DateOnly monday, DateOnly saturday) StartEndDay(DateOnly date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            var monday = date.AddDays(-1 * diff);
            var saturday = monday.AddDays(5);

            return (monday, saturday);
        }

        public async Task<TodayScheduleResult> GetTodayScheduleAsync(Guid userId, DateOnly? date)
        {
            var actualDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);

            var results = await _scheduleRepository.GetScheduleAsync(userId, actualDate, actualDate);

            return results.FirstOrDefault() ?? new TodayScheduleResult(
                Date: actualDate.ToString("yyyy-MM-dd"),
                DayName: actualDate.ToString("dddd"),
                WeekNumber: null,
                LessonsWeek: 0,
                Items: Array.Empty<ScheduleLessonsResult>()
            );
        }

        private static int GetIsoWeek(DateOnly date)
        {
            var day = date.ToDateTime(TimeOnly.MinValue);
            return System.Globalization.ISOWeek.GetWeekOfYear(day);
        }
    }
}
