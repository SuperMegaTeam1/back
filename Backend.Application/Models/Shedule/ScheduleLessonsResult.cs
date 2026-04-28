using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models.Shedule
{
    public sealed record ScheduleLessonsResult(
        Guid LessonsId,
        Guid SubjectId,
        string SubjectName,
        Guid? TeacherId,
        string? TeacherFirstName,
        string? TeacherLastName,
        string? TeacherFatherName,
        Guid? GroupId,
        string? GroupName,
        string? Cabinet,
        string? Type,
        string StartsAt,
        string EndsAt);
}
