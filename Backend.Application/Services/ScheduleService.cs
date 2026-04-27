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

        public async Task<TodayScheduleResult> GetTodayScheduleAsync(Guid userId, DateOnly? date)
        {
            var actualDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);

            var items = await _scheduleRepository.GetTodayScheduleAsync(userId, date);

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
