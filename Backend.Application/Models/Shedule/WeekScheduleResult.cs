using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models.Shedule
{
    public sealed record WeekScheduleResult(
        string DateStart,
        string DateEnd,
        IReadOnlyCollection<TodayScheduleResult> Items);
}
