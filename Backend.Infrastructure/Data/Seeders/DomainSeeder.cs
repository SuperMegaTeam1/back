using Backend.Domain.Entities;
using Backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Data.Seeders
{
    public static class DomainSeeder
    {
        public static async Task SeedDomainData(
            AppDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            var teacherUser = await userManager.FindByEmailAsync("teacher@test.com");
            var studentUser = await userManager.FindByEmailAsync("student@test.com");

            var teacher = await db.Teachers
                .FirstOrDefaultAsync(t => t.ParentUserId == teacherUser!.Id);

            if (teacher == null)
            {
                teacher = new Teacher
                {
                    Id = Guid.NewGuid(),
                    ParentUserId = teacherUser.Id,
                    FirstName = teacherUser.FirstName!,
                    LastName = teacherUser.LastName!,
                    FatherName = teacherUser.FatherName ?? "Unknown",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                db.Teachers.Add(teacher);
                await db.SaveChangesAsync();
            }

            var student = await db.Students
                .FirstOrDefaultAsync(s => s.ParentUserId == studentUser!.Id);

            if (student == null)
            {
                student = new Student
                {
                    Id = Guid.NewGuid(),
                    ParentUserId = studentUser.Id,
                    FirstName = studentUser.FirstName!,
                    LastName = studentUser.LastName!,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                db.Students.Add(student);
                await db.SaveChangesAsync();
            }

            var group = await db.StudyGroups
                .FirstOrDefaultAsync(g => g.Name == "Group A");

            if (group == null)
            {
                group = new StudyGroup
                {
                    Id = Guid.NewGuid(),
                    Name = "Group A"
                };

                db.StudyGroups.Add(group);
                await db.SaveChangesAsync();
            }

            if (student.GroupId != group.Id)
            {
                student.GroupId = group.Id;
                await db.SaveChangesAsync();
            }

            var subject = await db.Subjects
                .FirstOrDefaultAsync(s => s.Name == "Math");

            if (subject == null)
            {
                subject = new SubjectEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Math",
                    TeacherId = teacher.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                db.Subjects.Add(subject);
                await db.SaveChangesAsync();
            }

            var lesson = await db.Lessons
                .FirstOrDefaultAsync(l => l.SubjectId == subject.Id);

            if (lesson == null)
            {
                lesson = new Lesson
                {
                    Id = Guid.NewGuid(),
                    StudyGroupId = group.Id, 
                    TeacherId = teacher.Id,
                    SubjectId = subject.Id,
                    StartsAt = DateTime.UtcNow.AddDays(1),
                    EndsAt = DateTime.UtcNow.AddDays(1).AddHours(2),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                db.Lessons.Add(lesson);
                await db.SaveChangesAsync();
            }

            if (!await db.LessonParticipations.AnyAsync(x =>
                    x.StudentId == student.Id && x.LessonId == lesson.Id))
            {
                db.LessonParticipations.Add(new LessonParticipation
                {
                    StudentId = student.Id,
                    LessonId = lesson.Id,
                    Attended = true
                });

                await db.SaveChangesAsync();
            }

            if (!await db.StudentGrades.AnyAsync(x =>
                    x.StudentId == student.Id && x.LessonId == lesson.Id))
            {
                db.StudentGrades.Add(new StudentGrade
                {
                    StudentId = student.Id,
                    LessonId = lesson.Id,
                    Grade = 5
                });

                await db.SaveChangesAsync();
            }

            if (!await db.StudentRatings.AnyAsync(x =>
                    x.StudentId == student.Id && x.SubjectId == subject.Id))
            {
                db.StudentRatings.Add(new StudentRating
                {
                    StudentId = student.Id,
                    SubjectId = subject.Id,
                    Rating = 5
                });

                await db.SaveChangesAsync();
            }
        }
    }
}
