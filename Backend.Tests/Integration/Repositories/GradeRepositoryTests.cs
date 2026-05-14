using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repositories;
using Backend.Tests.Integration.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests.Integration.Repositories
{
    public class GradeRepositoryTests : IClassFixture<PostgresContainerFixture>
    {
        private readonly AppDbContext _db;
        private readonly GradeRepository _gradeRepository;

        public GradeRepositoryTests(PostgresContainerFixture postgresFixture)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(postgresFixture.Container.GetConnectionString())
                .Options;
            _db = new AppDbContext(options);
            _db.Database.Migrate();
            _gradeRepository = new GradeRepository(_db);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnGrade()
        {
            var grade = new StudentGrade
            {
                Id = Guid.NewGuid(),
                Grade = 5,
                StudentId = Guid.NewGuid(),
                LessonId = Guid.NewGuid()
            };
            _db.StudentGrades.Add(grade);
            await _db.SaveChangesAsync();
            var result = await _gradeRepository.GetByIdAsync(grade.Id);
            Assert.NotNull(result);
            Assert.Equal(grade.Id, result!.Id);
            Assert.Equal(grade.Grade, result.Grade);
        }

        [Fact]
        public async Task GetByGroupAsync_ShouldReturnOnlyGroupGrades()
        {
            var group1 = new StudyGroup
            {
                Id = Guid.NewGuid(),
                Name = "A"
            };

            var group2 = new StudyGroup
            {
                Id = Guid.NewGuid(),
                Name = "B"
            };

            var student1 = new Student
            {
                Id = Guid.NewGuid(),
                StudyGroupId = group1.Id
            };

            var student2 = new Student
            {
                Id = Guid.NewGuid(),
                StudyGroupId = group2.Id
            };

            var grade1 = new StudentGrade
            {
                Id = Guid.NewGuid(),
                Student = student1
            };

            var grade2 = new StudentGrade
            {
                Id = Guid.NewGuid(),
                Student = student2
            };

            _db.AddRange(group1, group2);
            _db.AddRange(student1, student2);
            _db.AddRange(grade1, grade2);

            await _db.SaveChangesAsync();


            var result = await _gradeRepository.GetByGroupAsync(group1.Id);


            result.Should().HaveCount(1);

            result.First().Student.StudyGroupId
                .Should().Be(group1.Id);
        }

        [Fact]
        public async Task GetByGroupAsync_ShouldIncludeStudent()
        {
            var student = new Student
            {
                Id = Guid.NewGuid()
            };

            var grade = new StudentGrade
            {
                Id = Guid.NewGuid(),
                Student = student
            };

            _db.StudentGrades.Add(grade);

            await _db.SaveChangesAsync();

            var result = await _gradeRepository.GetByGroupAsync(student.StudyGroupId);

            result.First().Student.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var result = await _gradeRepository.GetByIdAsync(Guid.NewGuid());

            result.Should().BeNull();
        }
    }
}
