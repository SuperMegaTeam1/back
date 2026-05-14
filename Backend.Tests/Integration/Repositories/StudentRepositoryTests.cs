using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repositories;
using Backend.Tests.Integration.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests.Integration.Repositories
{
    public class StudentRepositoryTests : IClassFixture<PostgresContainerFixture>
    {
        private readonly AppDbContext _db;
        private readonly StudentRepository _studentRepository;

        public StudentRepositoryTests(PostgresContainerFixture postgresFixture)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(postgresFixture.Container.GetConnectionString())
                .Options;
            _db = new AppDbContext(options);
            _db.Database.Migrate();
            _studentRepository = new StudentRepository(_db);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnStudent()
        {
            var group = new StudyGroup
            {
                Id = Guid.NewGuid(),
                Name = "GROUP_1"
            };

            var student = new Student
            {
                Id = Guid.NewGuid(),
                ParentUserId = Guid.NewGuid(),
                StudyGroupId = Guid.NewGuid()
            };

            _db.StudyGroups.Add(group);
            _db.Students.Add(student);

            await _db.SaveChangesAsync();

            var result = await _studentRepository.GetByIdAsync(student.Id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(student.Id);
            result.StudyGroup!.Name.Should().Be("GROUP_1");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenStudentDoesNotExist()
        {
            var randomId = Guid.NewGuid();

            var result = await _studentRepository.GetByIdAsync(randomId);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByGroupIdAsync_ShouldReturnOnlyStudentsFromRequestedGroup()
        {
            var group1 = new StudyGroup
            {
                Id = Guid.NewGuid(),
                Name = "GROUP_21"
            };

            var group2 = new StudyGroup
            {
                Id = Guid.NewGuid(),
                Name = "GROUP_22"
            };

            var student1 = new Student
            {
                Id = Guid.NewGuid(),
                ParentUserId = Guid.NewGuid(),
                StudyGroupId = group1.Id
            };

            var student2 = new Student
            {
                Id = Guid.NewGuid(),
                ParentUserId = Guid.NewGuid(),
                StudyGroupId = group1.Id
            };

            var student3 = new Student
            {
                Id = Guid.NewGuid(),
                ParentUserId = Guid.NewGuid(),
                StudyGroupId = group2.Id
            };

            _db.StudyGroups.AddRange(group1, group2);

            _db.Students.AddRange(student1, student2, student3);

            await _db.SaveChangesAsync();

            var result = await _studentRepository.GetByGroupIdAsync(group1.Id);

            result.Should().HaveCount(2);

            result.Should().OnlyContain(
                x => x.StudyGroupId == group1.Id);
        }
    }
}
