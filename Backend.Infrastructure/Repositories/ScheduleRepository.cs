using Backend.Application.Interfaces;
using Backend.Application.Models.Shedule;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public sealed class ScheduleRepository : IScheduleRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;

        public ScheduleRepository(
            UserManager<ApplicationUser> userManager,
            AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<TodayScheduleResult>> GetScheduleAsync(
            Guid userId, 
            DateOnly from, 
            DateOnly to)
        {
            var fromUtc = DateTime.SpecifyKind(from.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
            var toUtc = DateTime.SpecifyKind(to.AddDays(1).ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);

            var student = await _dbContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ParentUserId == userId);

            var teacher = student == null 
                ? await _dbContext.Teachers.AsNoTracking().FirstOrDefaultAsync(x => x.ParentUserId == userId)
                : null;

            if (student == null && teacher == null) return [];

            var query = _dbContext.Lessons
                .AsNoTracking()
                .Include(x => x.Subject)
                .Include(x => x.Teacher)
                .Include(x => x.StudyGroup)
                .Where(x => x.StartsAt >= fromUtc && x.StartsAt < toUtc);

            if (student != null)
                query = query.Where(x => x.StudyGroupId == student.StudyGroupId);
            else
                query = query.Where(x => x.TeacherId == teacher.Id);

            var allLessons = await query.OrderBy(x => x.StartsAt).ToListAsync();

            var results = new List<TodayScheduleResult>();

            for (var day = from; day <= to; day = day.AddDays(1))
            {
                var dateStr = day.ToString("yyyy-MM-dd");
                
                var lessonsForDay = allLessons
                    .Where(l => DateOnly.FromDateTime(l.StartsAt) == day)
                    .Select(MapToResult)
                    .ToList();

                results.Add(new TodayScheduleResult(
                    Date: dateStr,
                    DayName: day.ToString("dddd"),
                    WeekNumber: System.Globalization.ISOWeek.GetWeekOfYear(day.ToDateTime(TimeOnly.MinValue)),
                    LessonsWeek: lessonsForDay.Count,
                    Items: lessonsForDay
                ));
            }

            return results;
        }

        private static ScheduleLessonsResult MapToResult(Lesson lesson)
        {
            return new ScheduleLessonsResult(
                LessonsId: lesson.Id,
                SubjectId: lesson.SubjectId,
                SubjectName: lesson.Subject.Name,
                TeacherId: lesson.TeacherId,
                TeacherFirstName: lesson.Teacher?.FirstName,
                TeacherLastName: lesson.Teacher?.LastName,
                TeacherFatherName: lesson.Teacher?.FatherName,
                GroupId: lesson.StudyGroupId,
                GroupName: lesson.StudyGroup?.Name,
                Cabinet: null, 
                Type: null,
                StartsAt: lesson.StartsAt.ToString("HH:mm"),
                EndsAt: lesson.EndsAt.ToString("HH:mm")
            );
        }
    }
}
