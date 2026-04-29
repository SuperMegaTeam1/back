using Backend.Application.Models.Shedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IScheduleRepository
    {
        Task<IReadOnlyCollection<TodayScheduleResult>> GetWeekScheduleAsync(Guid userId, DateOnly monday, DateOnly saturday);
        Task<IReadOnlyCollection<ScheduleLessonsResult>> GetTodayScheduleAsync(Guid userId, DateOnly? date);
    }
}
