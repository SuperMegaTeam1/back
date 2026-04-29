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

        public async Task<IReadOnlyCollection<TodayScheduleResult>> GetWeekScheduleAsync(Guid userId, DateOnly monday, DateOnly saturday)
        {
            var schedule = new List<TodayScheduleResult>();

            for (var day = monday; day <= saturday; day = day.AddDays(1))
            {
                var ScheduleDay = await GetTodayScheduleAsync(userId, day);

                schedule.Add(
                    new TodayScheduleResult(
                        Date: day.ToString("yyyy-MM-dd"),
                        DayName : day.ToString("dddd"),
                        WeekNumber: null,
                        LessonsWeek: null,
                        Items : ScheduleDay
                    )
                );
            }

            return schedule;
        }

        public async Task<IReadOnlyCollection<ScheduleLessonsResult>> GetTodayScheduleAsync(Guid userId, DateOnly? date)
        {
            var targetDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return [];

            var student = await _dbContext.Students
                .Include(x => x.StudyGroup)
                .FirstOrDefaultAsync(x => x.ParentUserId == user.Id);

            if (student is not null)
            {
                return await GetStudentSchedule(student, targetDate);
            }

            var teacher = await _dbContext.Teachers
                .FirstOrDefaultAsync(x => x.ParentUserId == user.Id);

            if (teacher is not null)
            {
                return await GetTeacherSchedule(teacher, targetDate);
            }

            return [];
        }

        private async Task<IReadOnlyCollection<ScheduleLessonsResult>> GetStudentSchedule(Student student, DateOnly targetDate)
        {
            var lessons = await _dbContext.Lessons
                .Include(x => x.Subject)
                .Include(x => x.Teacher)
                .Include(x => x.StudyGroup)
                .Where(x => x.StudyGroupId == student.StudyGroupId)
                .Where(x => DateOnly.FromDateTime(x.StartsAt) == targetDate)
                .OrderBy(x => x.StartsAt)
                .ToListAsync();

            return lessons.Select(lesson => new ScheduleLessonsResult(
                LessonsId: lesson.Id,
                SubjectId: lesson.SubjectId,
                SubjectName: lesson.Subject.Name,
                TeacherId: lesson.TeacherId,
                TeacherFirstName: lesson.Teacher.FirstName,
                TeacherLastName: lesson.Teacher.LastName,
                TeacherFatherName: lesson.Teacher.FatherName,
                GroupId: lesson.StudyGroupId,
                GroupName: lesson.StudyGroup.Name,
                Cabinet: null,
                Type: null,
                StartsAt: lesson.StartsAt.ToString("HH:mm"),
                EndsAt: lesson.EndsAt.ToString("HH:mm")
            )).ToList();
        }

        private async Task<IReadOnlyCollection<ScheduleLessonsResult>> GetTeacherSchedule(Teacher teacher, DateOnly targetDate)
        {
            var lessons = await _dbContext.Lessons
                .Include(x => x.Subject)
                .Include(x => x.StudyGroup)
                .Where(x => x.TeacherId == teacher.Id)
                .Where(x => DateOnly.FromDateTime(x.StartsAt) == targetDate)
                .OrderBy(x => x.StartsAt)
                .ToListAsync();

            return lessons.Select(lesson => new ScheduleLessonsResult(
                LessonsId: lesson.Id,
                SubjectId: lesson.SubjectId,
                SubjectName: lesson.Subject.Name,
                TeacherId: teacher.Id,
                TeacherFirstName: teacher.FirstName,
                TeacherLastName: teacher.LastName,
                TeacherFatherName: teacher.FatherName,
                GroupId: lesson.StudyGroupId,
                GroupName: lesson.StudyGroup.Name,
                Cabinet: null,
                Type: null,
                StartsAt: lesson.StartsAt.ToString("HH:mm"),
                EndsAt: lesson.EndsAt.ToString("HH:mm")
            )).ToList();
        }
    }
}
