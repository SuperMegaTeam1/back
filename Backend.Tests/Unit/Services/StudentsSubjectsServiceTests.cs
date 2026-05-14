using Backend.Application.Interfaces;
using Backend.Application.Services;
using Backend.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Backend.Tests.Unit.Services;

public class StudentsSubjectsServiceTests
{
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock;

    private readonly StudentsSubjectsService _service;

    public StudentsSubjectsServiceTests()
    {
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _subjectRepositoryMock = new Mock<ISubjectRepository>();

        _service = new StudentsSubjectsService(
            _studentRepositoryMock.Object,
            _subjectRepositoryMock.Object
        );
    }

    [Fact]
    public async Task GetSubjectsForStudentAsync_ShouldReturnSubjects()
    {
        var userId = Guid.NewGuid();
        var groupId = Guid.NewGuid();

        var student = new Student
        {
            Id = Guid.NewGuid(),
            ParentUserId = userId,
            StudyGroupId = groupId
        };

        var subjects = new List<SubjectEntity>
        {
            new SubjectEntity
            {
                Id = Guid.NewGuid(),
                Name = "Math"
            },
            new SubjectEntity
            {
                Id = Guid.NewGuid(),
                Name = "Physics"
            }
        };

        _studentRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(student);

        _subjectRepositoryMock
            .Setup(x => x.GetSubjectsByStudyGroupIdAsync(groupId))
            .ReturnsAsync(subjects);

        var result = await _service.GetSubjectsForStudentAsync(userId);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result.Should().Contain(x => x.SubjectName == "Math");
        result.Should().Contain(x => x.SubjectName == "Physics");
    }

    [Fact]
    public async Task GetSubjectsForStudentAsync_ShouldThrowException_WhenStudentNotFound()
    {
        var userId = Guid.NewGuid();

        _studentRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync((Student?)null);

        Func<Task> act = async () =>
            await _service.GetSubjectsForStudentAsync(userId);

        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Student not found");
    }

    [Fact]
    public async Task GetSubjectsForStudentBySubjectIdAsync_ShouldReturnOnlyRequestedSubject()
    {
        var userId = Guid.NewGuid();
        var groupId = Guid.NewGuid();

        var neededSubjectId = Guid.NewGuid();

        var student = new Student
        {
            Id = Guid.NewGuid(),
            ParentUserId = userId,
            StudyGroupId = groupId
        };

        var subjects = new List<SubjectEntity>
        {
            new SubjectEntity
            {
                Id = neededSubjectId,
                Name = "Math"
            },
            new SubjectEntity
            {
                Id = Guid.NewGuid(),
                Name = "Physics"
            }
        };

        _studentRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(student);

        _subjectRepositoryMock
            .Setup(x => x.GetSubjectsByStudyGroupIdAsync(groupId))
            .ReturnsAsync(subjects);

        var result = await _service
            .GetSubjectsForStudentBySubjectIdAsync(
                userId,
                neededSubjectId);

        result.Should().HaveCount(1);

        result.First().SubjectName.Should().Be("Math");
    }

    [Fact]
    public async Task GetSubjectsForStudentBySubjectIdAsync_ShouldReturnEmptyList_WhenSubjectNotFound()
    {
        var userId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var student = new Student
        {
            Id = Guid.NewGuid(),
            ParentUserId = userId,
            StudyGroupId = groupId
        };
        var subjects = new List<SubjectEntity>
        {
            new SubjectEntity
            {
                Id = Guid.NewGuid(),
                Name = "Math"
            },
            new SubjectEntity
            {
                Id = Guid.NewGuid(),
                Name = "Physics"
            }
        };
        _studentRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(student);
        _subjectRepositoryMock
            .Setup(x => x.GetSubjectsByStudyGroupIdAsync(groupId))
            .ReturnsAsync(subjects);
        var result = await _service
            .GetSubjectsForStudentBySubjectIdAsync(
                userId,
                Guid.NewGuid());
        result.Should().BeEmpty();
    }
}