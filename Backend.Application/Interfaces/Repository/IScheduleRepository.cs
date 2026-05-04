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
        // todo объяед в один мето
        Task<IReadOnlyCollection<ScheduleLessonsResult>> GetScheduleAsync(Guid userId, DateOnly from, DateOnly to);
    }
}
