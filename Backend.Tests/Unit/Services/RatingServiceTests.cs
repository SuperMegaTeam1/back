using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Repository;
using Backend.Application.Services;
using Backend.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Backend.Tests.Unit.Services;

public class RatingServiceTests
{
    private readonly Mock<IStudentRepository> _studentRepoMock;

    private readonly Mock<IGradeRepository> _gradeRepoMock;

    private readonly Mock<ISubjectRepository> _subjectRepoMock;

    private readonly Mock<IStudentRatingRepository> _ratingRepoMock;

    private readonly Mock<IStudyGroupRepository> _studyGroupRepoMock;

    private readonly Mock<ILessonRepository> _lessonRepoMock;

    private readonly RatingService _service;

    public RatingServiceTests()
    {
        _studentRepoMock =
            new Mock<IStudentRepository>();

        _gradeRepoMock =
            new Mock<IGradeRepository>();

        _subjectRepoMock =
            new Mock<ISubjectRepository>();

        _ratingRepoMock =
            new Mock<IStudentRatingRepository>();

        _studyGroupRepoMock =
            new Mock<IStudyGroupRepository>();

        _lessonRepoMock =
            new Mock<ILessonRepository>();

        _service = new RatingService(
            _studentRepoMock.Object,
            _gradeRepoMock.Object,
            _subjectRepoMock.Object,
            _ratingRepoMock.Object,
            _studyGroupRepoMock.Object,
            _lessonRepoMock.Object);
    }

    [Fact]
    public async Task GetMyRatingAsync_ShouldThrow_WhenStudentNotFound()
    {

        var userId = Guid.NewGuid();

        _studentRepoMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync((Student?)null);


        Func<Task> act = async () =>
            await _service.GetMyRatingAsync(userId, null);


        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Student not found");
    }

    [Fact]
    public async Task GetMyRatingAsync_ShouldUseSubjectRatings_WhenSubjectIdProvided()
    {

        var userId = Guid.NewGuid();

        var subjectId = Guid.NewGuid();

        var student = new Student
        {
            Id = Guid.NewGuid(),

            StudyGroupId = Guid.NewGuid(),

            StudyGroup = new StudyGroup
            {
                Name = "PI-21"
            }
        };

        _studentRepoMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(student);

        _ratingRepoMock
            .Setup(x => x.GetByGroupAndSubjectAsync(
                student.StudyGroupId,
                subjectId))
            .ReturnsAsync(new List<StudentRating>());


        await _service.GetMyRatingAsync(
            userId,
            subjectId);


        _ratingRepoMock.Verify(
            x => x.GetByGroupAndSubjectAsync(
                student.StudyGroupId,
                subjectId),
            Times.Once);
    }

    [Fact]
    public async Task UpdateRatingsAsync_ShouldCreateRatings()
    {

        var group = new StudyGroup
        {
            Id = Guid.NewGuid(),
            Name = "PI-21"
        };

        var lessons = new List<Lesson>
        {
            new Lesson
            {
                SubjectId = Guid.NewGuid()
            }
        };

        var student1 = new Student
        {
            Id = Guid.NewGuid()
        };

        var student2 = new Student
        {
            Id = Guid.NewGuid()
        };

        var grades = new List<StudentGrade>
    {
            new StudentGrade
            {
                StudentId = student1.Id,
                Grade = 5
            },

            new StudentGrade
            {
                StudentId = student2.Id,
                Grade = 3
            }
        };

        _studyGroupRepoMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<StudyGroup> { group });

        _lessonRepoMock
            .Setup(x => x.GetLessonsByStudyGroup(group.Id))
            .ReturnsAsync(lessons);

        _gradeRepoMock
            .Setup(x => x.GetByGroupAsync(group.Id))
            .ReturnsAsync(grades);

        _gradeRepoMock
            .Setup(x => x.GetByGroupAndSubjectAsync(
                group.Id,
                It.IsAny<Guid>()))
            .ReturnsAsync(grades);


        await _service.UpdateRatingsAsync();


        _ratingRepoMock.Verify(
            x => x.ClearAsync(),
            Times.Once);

        _ratingRepoMock.Verify(
            x => x.AddRangeAsync(
                It.Is<List<StudentRating>>(r =>
                    r.Count > 0)),
            Times.Once);
    }
}