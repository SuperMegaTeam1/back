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
        private static async Task<SubjectEntity> GetOrCreateSubject(
            AppDbContext db,
            string name,
            Guid teacherId)
        {
            var subject = await db.Subjects.FirstOrDefaultAsync(s => s.Name == name);

            if (subject == null)
            {
                subject = new SubjectEntity
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    TeacherId = teacherId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                db.Subjects.Add(subject);
                await db.SaveChangesAsync();
            }

            return subject;
        }
        public static async Task SeedDomainData(
            AppDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            var teacherUser = await userManager.FindByEmailAsync("teacher@test.com");
            var studentUser = await userManager.FindByEmailAsync("student@test.com");

            var teacher = await db.Teachers.FirstOrDefaultAsync();

            if (teacher == null)
            {
                teacher = new Teacher
                {
                    Id = Guid.NewGuid(),
                    ParentUserId = teacherUser!.Id,
                    FirstName = teacherUser.FirstName!,
                    LastName = teacherUser.LastName!,
                    FatherName = teacherUser.FatherName,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                db.Teachers.Add(teacher);
                await db.SaveChangesAsync();
            }

            var groupA = await db.StudyGroups.FirstOrDefaultAsync(g => g.Name == "Group A");
            if (groupA == null)
            {
                groupA = new StudyGroup { Id = Guid.NewGuid(), Name = "Group A" };
                db.StudyGroups.Add(groupA);
            }

            var groupB = await db.StudyGroups.FirstOrDefaultAsync(g => g.Name == "Group B");
            if (groupB == null)
            {
                groupB = new StudyGroup { Id = Guid.NewGuid(), Name = "Group B" };
                db.StudyGroups.Add(groupB);
            }

            await db.SaveChangesAsync();

            var math = await GetOrCreateSubject(db, "Матанализ", teacher.Id);
            var physics = await GetOrCreateSubject(db, "Физика", teacher.Id);

            var students = new List<Student>();

            for (int i = 1; i <= 5; i++)
            {
                var user = await userManager.FindByEmailAsync($"student{i}@test.com");

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = $"student{i}@test.com",
                        Email = $"student{i}@test.com",
                        FirstName = $"Student{i}",
                        LastName = "Test"
                    };

                    await userManager.CreateAsync(user, "Password123!");
                    await userManager.AddToRoleAsync(user, "Student");
                }

                var student = await db.Students.FirstOrDefaultAsync(s => s.ParentUserId == user.Id);

                if (student == null)
                {
                    student = new Student
                    {
                        Id = Guid.NewGuid(),
                        ParentUserId = user.Id,
                        FirstName = user.FirstName!,
                        LastName = user.LastName!,
                        StudyGroupId = i % 2 == 0 ? groupA.Id : groupB.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    db.Students.Add(student);
                }

                students.Add(student);
            }

            await db.SaveChangesAsync();

            var lesson = new Lesson
            {
                Id = Guid.NewGuid(),
                StudyGroupId = groupA.Id,
                TeacherId = teacher.Id,
                SubjectId = math.Id,
                StartsAt = DateTime.UtcNow,
                EndsAt = DateTime.UtcNow.AddHours(2),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            db.Lessons.Add(lesson);
            await db.SaveChangesAsync();

            var rnd = new Random();

            foreach (var student in students)
            {
                if (!db.StudentGrades.Any(g => g.StudentId == student.Id))
                {
                    db.StudentGrades.Add(new StudentGrade
                    {
                        StudentId = student.Id,
                        LessonId = lesson.Id,
                        Grade = rnd.Next(60, 100)
                    });
                }
            }

            await db.SaveChangesAsync();
        }
    }
}