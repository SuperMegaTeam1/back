using Backend.Domain.Entities;
using Backend.Infrastructure.Repositories;
using Backend.Tests.Integration.Fixtures;
using FluentAssertions;
using Xunit;

namespace Backend.Tests.Integration.Repositories;

public class LessonRepositoryTests : IClassFixture<DatabaseFixture>, IAsyncLifetime
{
    private readonly DatabaseFixture _fixture;
    private readonly LessonRepository _repo;

    public LessonRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _repo = new LessonRepository(fixture.DbContext);
    }

    public async Task InitializeAsync() => await _fixture.ResetDatabase();
    public Task DisposeAsync() => Task.CompletedTask;

    private async Task<(
        Lesson lesson, 
        Teacher teacher, 
        StudyGroup group, 
        SubjectEntity subject)> SeedLesson(Guid? teacherUserId = null)
    {
        var userId = teacherUserId ?? Guid.NewGuid();

        var teacher = new Teacher
        {
            Id = Guid.NewGuid(),
            ParentUserId = userId,
            FirstName = "Иван",
            LastName = "Иванов"
        };

        var group = new StudyGroup
        {
            Id = Guid.NewGuid(),
            Name = "Группа 1"
        };

        var subject = new SubjectEntity
        {
            Id = Guid.NewGuid(),
            Name = "Математика",
            TeacherId = teacher.Id
        };

        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            TeacherId = teacher.Id,
            StudyGroupId = group.Id,
            SubjectId = subject.Id,
            StartsAt = DateTime.UtcNow,
            EndsAt = DateTime.UtcNow.AddHours(1)
        };

        _fixture.DbContext.Teachers.Add(teacher);
        _fixture.DbContext.StudyGroups.Add(group);
        await _fixture.DbContext.SaveChangesAsync(); 

        _fixture.DbContext.Subjects.Add(subject);
        await _fixture.DbContext.SaveChangesAsync(); 

        _fixture.DbContext.Lessons.Add(lesson);
        await _fixture.DbContext.SaveChangesAsync();

        return (lesson, teacher, group, subject);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnLesson_WhenExists()
    {
        var (lesson, _, _, _) = await SeedLesson();

        var result = await _repo.GetByIdAsync(lesson.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(lesson.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        var result = await _repo.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetLessonsByStudyGroup_ShouldReturnLessons_ForGroup()
    {
        var (lesson, _, group, _) = await SeedLesson();

        var result = await _repo.GetLessonsByStudyGroup(group.Id);

        result.Should().HaveCount(1);
        result[0]!.StudyGroupId.Should().Be(group.Id);
    }

    [Fact]
    public async Task GetLessonsByStudyGroup_ShouldReturnEmpty_WhenNoLessons()
    {
        var result = await _repo.GetLessonsByStudyGroup(Guid.NewGuid());

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetLessonsByStudyGroupAndSubject_ShouldReturnLesson_WhenMatches()
    {
        var (lesson, _, group, subject) = await SeedLesson();

        var result = await _repo.GetLessonsByStudyGroupAndSubject(group.Id, subject.Id);

        result.Should().HaveCount(1);
        result[0]!.SubjectId.Should().Be(subject.Id);
    }

    [Fact]
    public async Task GetLessonsByStudyGroupAndSubject_ShouldReturnEmpty_WhenNoMatch()
    {
        var (_, _, group, _) = await SeedLesson();

        var result = await _repo.GetLessonsByStudyGroupAndSubject(group.Id, Guid.NewGuid());

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetLessonsByTeacherSubjectAndStudyGroup_ShouldReturnLesson_WithGradesAndParticipations()
    {
        var userId = Guid.NewGuid();
        var (lesson, _, group, subject) = await SeedLesson(userId);

        var result = await _repo.GetLessonsByTeacherSubjectAndStudyGroup(subject.Id, group.Id, userId);

        result.Should().HaveCount(1);
        result[0]!.Id.Should().Be(lesson.Id);
        result[0]!.Grades.Should().NotBeNull();
        result[0]!.Participations.Should().NotBeNull();
    }

    [Fact]
    public async Task GetLessonsByTeacherSubjectAndStudyGroup_ShouldReturnEmpty_WhenWrongUserId()
    {
        var (_, _, group, subject) = await SeedLesson();

        var result = await _repo.GetLessonsByTeacherSubjectAndStudyGroup(subject.Id, group.Id, Guid.NewGuid());

        result.Should().BeEmpty();
    }
}