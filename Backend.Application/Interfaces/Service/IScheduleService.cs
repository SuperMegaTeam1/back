using Backend.Application.Models.Shedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IScheduleService
    {
        Task<TodayScheduleResult> GetTodayScheduleAsync(Guid userId, DateOnly? date);
        Task<WeekScheduleResult> GetWeekScheduleAsync(Guid userId, DateOnly? date);
    }
}

// так значит делаем следующим образом 
// тут оставляем два метода а в репозиториивызываем один метод т.к. репо будет делать в зависимости от параметров день или неделя