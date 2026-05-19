using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Models.Group;

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
        List<StudyGroupDto>? StudyGroups,
        string StartsAt,
        string EndsAt);
}
