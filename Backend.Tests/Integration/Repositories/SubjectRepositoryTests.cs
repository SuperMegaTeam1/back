using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repositories;
using Backend.Tests.Integration.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests.Integration.Repositories
{
    public class SubjectRepositoryTests
    {
        private readonly AppDbContext _db;
        private readonly SubjectRepository _subjectRepository;

        public SubjectRepositoryTests(PostgresContainerFixture postgresFixture)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(postgresFixture.Container.GetConnectionString())
                .Options;
            _db = new AppDbContext(options);
            _db.Database.Migrate();
            _subjectRepository = new SubjectRepository(_db);
        }

        [Fact]
        public async Task GetNameByIdAsync_ShouldReturnSubjectName()
        {
            var subject = new SubjectEntity
            {
                Id = Guid.NewGuid(),
                Name = "Math"
            };
            _db.Subjects.Add(subject);
            await _db.SaveChangesAsync();
            var result = await _subjectRepository.GetNameByIdAsync(subject.Id);
            Assert.NotNull(result);
            Assert.Equal(subject.Name, result);
        }

        [Fact]
        public async Task GetSubjectsByStudyGroupIdAsync_ShouldReturnSubjects()
        {
            var group = new StudyGroup
            {
                Id = Guid.NewGuid(),
                Name = "GROUP_1"
            };
            var subject1 = new SubjectEntity
            {
                Id = Guid.NewGuid(),
                Name = "Math"
            };
            var subject2 = new SubjectEntity
            {
                Id = Guid.NewGuid(),
                Name = "Physics"
            };
            var lesson1 = new Lesson
            {
                Id = Guid.NewGuid(),
                StudyGroupId = group.Id,
                SubjectId = subject1.Id,
                TeacherId = Guid.NewGuid()
            };
            var lesson2 = new Lesson
            {
                Id = Guid.NewGuid(),
                StudyGroupId = group.Id,
                SubjectId = subject2.Id,
                TeacherId = Guid.NewGuid()
            };
            _db.StudyGroups.Add(group);
            _db.Subjects.AddRange(subject1, subject2);
            _db.Lessons.AddRange(lesson1, lesson2);
            await _db.SaveChangesAsync();
            var result = await _subjectRepository.GetSubjectsByStudyGroupIdAsync(group.Id);
            result.Should().HaveCount(2);
            result.Should().Contain(s => s.Name == "Math");
            result.Should().Contain(s => s.Name == "Physics");
        }
    }
}
