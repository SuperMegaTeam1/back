using Backend.Application.Interfaces;
using Backend.Application.Models.Shedule;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
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

        public async Task<IReadOnlyCollection<ScheduleLessonsResult>> GetTodayScheduleAsync(Guid userId, DateOnly? date)
        {
            var lessonsList = new List<ScheduleLessonsResult>();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return null;
            }

            var student = await _dbContext.Students.FirstOrDefaultAsync(x => x.ParentUserId == user.Id);

            if (student is null)
            {
                return null;
            }

            if (student.StudyGroup is not null)
            {
                var group = await _dbContext.StudyGroups.FirstOrDefaultAsync(x => x.Id == student.StudyGroupId);
                var lessons = await _dbContext.Lessons
                    .Where(x => x.StudyGroupId == group.Id) // todo доделать выборку по дате
                    .Where(x => DateOnly.FromDateTime(x.StartsAt) == date)
                    .ToListAsync();


                foreach (var lesson in lessons)
                {
                    var subject = await _dbContext.Subjects.FirstOrDefaultAsync(x => x.Id == lesson.SubjectId);
                    var teacherLesson = await _dbContext.Teachers.FirstOrDefaultAsync(x => x.Id == lesson.TeacherId);

                    if (subject is null || group is null)
                    {
                        continue;
                    }

                    lessonsList.Add(new ScheduleLessonsResult(
                        LessonsId: lesson.Id,
                        SubjectId: lesson.SubjectId,
                        SubjectName: subject.Name,
                        TeacherId: teacherLesson.Id,
                        TeacherFirstName: teacherLesson.FirstName,
                        TeacherLastName: teacherLesson.LastName,
                        TeacherFatherName: teacherLesson.FatherName,
                        GroupId: group.Id,
                        GroupName: group.Name,
                        Cabinet: null,
                        Type: null,
                        StartsAt: lesson.StartsAt.ToString("HH:mm"),
                        EndsAt: lesson.EndsAt.ToString("HH:mm")
                        ));
                }

                return lessonsList;
            }

            var teacher = await _dbContext.Teachers.FirstOrDefaultAsync(x => x.ParentUserId == user.Id);

            if (teacher is not null)
            {
                var lessons = await _dbContext.Lessons
                    .Where(x => x.TeacherId == teacher.Id)
                    .Where(x => DateOnly.FromDateTime(x.StartsAt) == date)
                    .OrderBy(x => x.StartsAt)
                    .ToListAsync();

                foreach (var lesson in lessons)
                {
                    var subject = await _dbContext.Subjects
                        .FirstOrDefaultAsync(x => x.Id == lesson.SubjectId);

                    var group = await _dbContext.StudyGroups
                        .FirstOrDefaultAsync(x => x.Id == lesson.StudyGroupId);

                    if (subject is null || group is null)
                        continue;

                    lessonsList.Add(new ScheduleLessonsResult(
                        LessonsId: lesson.Id,
                        SubjectId: lesson.SubjectId,
                        SubjectName: subject.Name,
                        TeacherId: teacher.Id,
                        TeacherFirstName: teacher.FirstName,
                        TeacherLastName: teacher.LastName,
                        TeacherFatherName: teacher.FatherName,
                        GroupId: group.Id,
                        GroupName: group.Name,
                        Cabinet: null,
                        Type: null,
                        StartsAt: lesson.StartsAt.ToString("HH:mm"),
                        EndsAt: lesson.EndsAt.ToString("HH:mm")
                    ));
                }

                return lessonsList;
            }

            return null;
        }
    }
}
