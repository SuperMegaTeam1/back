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

            var items = await _scheduleRepository.GetWeekScheduleAsync(userId, monday, saturday);
            
            if (items == null)
            {
                return null;
            }

            return new WeekScheduleResult(
                DateStart: monday.ToString(),
                DateEnd: saturday.ToString(),
                Items: items
            );
        }

        private (DateOnly monday, DateOnly saturday) StartEndDay(DateOnly date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            var monday = date.AddDays(-dayOfWeek + 1);
            var saturday = monday.AddDays(5);

            return (monday, saturday);
        }

        public async Task<TodayScheduleResult> GetTodayScheduleAsync(Guid userId, DateOnly? date)
        {
            var actualDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);

            var items = await _scheduleRepository.GetTodayScheduleAsync(userId, actualDate);

            if (items == null)
            {
                return null;
            }

            return new TodayScheduleResult(
                Date: actualDate.ToString("yyyy-MM-dd"),
                DayName: actualDate.DayOfWeek.ToString(),
                WeekNumber: GetIsoWeek(actualDate),
                LessonsWeek: items.Count,
                Items: items
                );
        }

        private static int GetIsoWeek(DateOnly date)
        {
            var day = date.ToDateTime(TimeOnly.MinValue);
            return System.Globalization.ISOWeek.GetWeekOfYear(day);
        }
    }
}
