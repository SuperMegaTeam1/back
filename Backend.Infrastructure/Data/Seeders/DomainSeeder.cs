using Backend.Domain.Entities;
using Backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Data.Seeders
{
    public static class DomainSeeder
    {
        private static readonly string[] GroupNames = { "09-351", "09-352", "09-353" };

        // Специальные студенты для группы 09-352
        private static readonly string[] SpecialStudents_352 =
        {
            "Камилла Гасимова Тимуровна",
            "Ростислав Врублевский Игоревич",
            "Алина Мазитова Рамилевна",
            "Тимур Закиров Айратович"
        };

        // Примерные имена для остальных студентов
        private static readonly string[] StudentFirstNames = { "Алексей", "Мария", "Сергей", "Екатерина", "Дмитрий", "Ольга", "Ирина", "Никита", "Владимир", "Анна" };
        private static readonly string[] StudentLastNames = { "Иванов", "Петрова", "Сидоров", "Смирнова", "Кузнецов", "Морозова", "Волкова", "Николаев", "Козлов", "Тимофеева" };
        private static readonly string[] StudentFatherNames = { "Иванович", "Евгеньевна", "Сергеевич", "Екатериновна", "Тимурович", "Олеговна", "Игоревна", "Никитич", "Владимирович", "Артуровна" };

        private static readonly string[] TeacherNames =
        {
            "Алексей Петрович Иванов",
            "Мария Сергеевна Смирнова",
            "Сергей Николаевич Сидоров",
            "Екатерина Дмитриевна Кузнецова",
            "Дмитрий Владимирович Морозов"
        };

        private static readonly string[] Subjects =
        {
            "Матанализ",
            "Физика",
            "Линейная алгебра",
            "Аналитическая геометрия",
            "Теория вероятности",
            "Программирование на C#",
            "Веб-программирование"
        };

        private static readonly TimeSpan[] LessonTimes =
        {
            new TimeSpan(8,30,0),
            new TimeSpan(10,10,0),
            new TimeSpan(11,50,0)
        };

        public static async Task SeedDomainData(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            var rnd = new Random();

            // ----------------- Создаем учителей -----------------
            var teachers = new List<Teacher>();
            for (int i = 0; i < TeacherNames.Length; i++)
            {
                var names = TeacherNames[i].Split(' ');
                var email = $"teacher{i + 1}@test.com";

                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FirstName = names[0],
                        FatherName = names[1],
                        LastName = names[2]
                    };
                    await userManager.CreateAsync(user, "Password123!");
                    await userManager.AddToRoleAsync(user, "Teacher");
                }

                var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.ParentUserId == user.Id);
                if (teacher == null)
                {
                    teacher = new Teacher
                    {
                        Id = Guid.NewGuid(),
                        ParentUserId = user.Id,
                        FirstName = user.FirstName!,
                        LastName = user.LastName!,
                        FatherName = user.FatherName,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    db.Teachers.Add(teacher);
                }
                teachers.Add(teacher);
            }
            await db.SaveChangesAsync();

            // ----------------- Создаем группы -----------------
            var groups = new List<StudyGroup>();
            foreach (var groupName in GroupNames)
            {
                var group = await db.StudyGroups.FirstOrDefaultAsync(g => g.Name == groupName);
                if (group == null)
                {
                    group = new StudyGroup
                    {
                        Id = Guid.NewGuid(),
                        Name = groupName
                    };
                    db.StudyGroups.Add(group);
                }
                groups.Add(group);
            }
            await db.SaveChangesAsync();

            // ----------------- Создаем студентов -----------------
            foreach (var group in groups)
            {
                for (int i = 0; i < 10; i++)
                {
                    string firstName, lastName, fatherName;
                    if (group.Name == "09-352" && i < SpecialStudents_352.Length)
                    {
                        var parts = SpecialStudents_352[i].Split(' ');
                        firstName = parts[0];
                        lastName = parts[1];
                        fatherName = parts[2];
                    }
                    else
                    {
                        firstName = StudentFirstNames[i % StudentFirstNames.Length];
                        lastName = StudentLastNames[i % StudentLastNames.Length];
                        fatherName = StudentFatherNames[i % StudentFatherNames.Length];
                    }

                    var email = $"student_{group.Name}_{i + 1}@test.com";

                    var user = await userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = email,
                            Email = email,
                            FirstName = firstName,
                            LastName = lastName,
                            FatherName = fatherName
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
                            FirstName = firstName,
                            LastName = lastName,
                            FatherName = fatherName,
                            StudyGroupId = group.Id,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        db.Students.Add(student);
                    }
                }
            }
            await db.SaveChangesAsync();

            // ----------------- Создаем предметы -----------------
            var subjectEntities = new List<SubjectEntity>();
            for (int i = 0; i < Subjects.Length; i++)
            {
                var name = Subjects[i];
                var teacher = teachers[i % teachers.Count];

                var subject = await db.Subjects.FirstOrDefaultAsync(s => s.Name == name && s.TeacherId == teacher.Id);
                if (subject == null)
                {
                    subject = new SubjectEntity
                    {
                        Id = Guid.NewGuid(),
                        Name = name,
                        TeacherId = teacher.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    db.Subjects.Add(subject);
                }
                subjectEntities.Add(subject);
            }
            await db.SaveChangesAsync();

            // ----------------- Создаем расписание на 2 недели -----------------
            var today = DateTime.UtcNow.Date;
            int totalDays = 14;
            var weekdays = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };

            for (int dayOffset = 0; dayOffset < totalDays; dayOffset++)
            {
                var date = today.AddDays(dayOffset);
                if (!weekdays.Contains(date.DayOfWeek))
                    continue;

                foreach (var group in groups)
                {
                    int pairsToday = rnd.Next(2, 4); // 2–3 пары в день
                    var usedTimes = new List<TimeSpan>();
                    for (int p = 0; p < pairsToday; p++)
                    {
                        TimeSpan time;
                        do
                        {
                            time = LessonTimes[rnd.Next(LessonTimes.Length)];
                        } while (usedTimes.Contains(time));
                        usedTimes.Add(time);

                        var teacher = teachers[rnd.Next(teachers.Count)];
                        var subject = subjectEntities.Where(s => s.TeacherId == teacher.Id).OrderBy(x => rnd.Next()).First();

                        var lesson = new Lesson
                        {
                            Id = Guid.NewGuid(),
                            StudyGroupId = group.Id,
                            TeacherId = teacher.Id,
                            SubjectId = subject.Id,
                            StartsAt = date.Add(time),
                            EndsAt = date.Add(time).AddMinutes(90),
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        db.Lessons.Add(lesson);
                    }
                }
            }
            await db.SaveChangesAsync();
        }
    }
}