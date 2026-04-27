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
        Task<IReadOnlyCollection<ScheduleLessonsResult>> GetTodayScheduleAsync(Guid userId, DateOnly? date);
    }
}
