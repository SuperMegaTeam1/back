using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models.Shedule
{
    public sealed record TodayScheduleResult(
        string Date,
        string DayName,
        int? WeekNumber,
        int? LessonsWeek,
        IReadOnlyCollection<ScheduleLessonsResult>? Items);
}
