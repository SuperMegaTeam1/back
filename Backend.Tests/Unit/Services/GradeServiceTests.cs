using Backend.Application.Interfaces;
using Backend.Application.Services;
using Backend.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Backend.Tests.Unit.Services
{
    public class GradeServiceTests
{
    private readonly Mock<IGradeRepository> _gradeRepo;
    private readonly Mock<IParticipationRepository> _participationRepo;
    private readonly Mock<INotificationRepository> _notificationRepo;
    private readonly Mock<INotificationSender> _notificationSender;
    private readonly Mock<IStudentRepository> _studentRepo;

    public GradeServiceTests()
    {
        _gradeRepo = new Mock<IGradeRepository>();
        _participationRepo = new Mock<IParticipationRepository>();
        _notificationRepo = new Mock<INotificationRepository>();
        _notificationSender = new Mock<INotificationSender>();
        _studentRepo = new Mock<IStudentRepository>();

        _studentRepo
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Student { Id = Guid.NewGuid(), ParentUserId = Guid.NewGuid() });
    }

    private GradeService CreateService() =>
        new GradeService(
            _gradeRepo.Object,
            _participationRepo.Object,
            _notificationRepo.Object,
            _notificationSender.Object,
            _studentRepo.Object);

    private void SetupParticipation(StudentGrade grade, LessonParticipation? existing = null)
    {
        _participationRepo
            .Setup(x => x.Get(grade.StudentId, grade.LessonId))
            .ReturnsAsync(existing);

        if (existing == null)
            _participationRepo
                .Setup(x => x.AddAsync(It.IsAny<LessonParticipation>()))
                .Returns(Task.CompletedTask);

        _participationRepo
            .Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task UpdateGrade_ShouldReturnUpdatedGrade()
    {
        var teacherId = Guid.NewGuid();
        var gradeEntity = new StudentGrade
        {
            Id = Guid.NewGuid(),
            Grade = 3,
            Lesson = new Lesson
            {
                StartsAt = new DateTime(2026, 6, 2),
                Subject = new SubjectEntity { Name = "Математика" }
            }
        };

        _gradeRepo.Setup(x => x.GetByIdAsync(gradeEntity.Id)).ReturnsAsync(gradeEntity);
        _gradeRepo.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);
        _notificationRepo.Setup(x => x.CreateNotificationAsync(It.IsAny<Notification>()))
            .Returns(Task.CompletedTask);
        SetupParticipation(gradeEntity);

        var result = await CreateService().UpdateGrade(teacherId, gradeEntity.Id, grade: 5, attended: null);

        result.Grade.Should().Be(5);
        _gradeRepo.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateGrade_ShouldThrowException_WhenGradeNotFound()
    {
        var gradeId = Guid.NewGuid();
        _gradeRepo.Setup(x => x.GetByIdAsync(gradeId)).ReturnsAsync((StudentGrade)null);

        Func<Task> act = async () =>
            await CreateService().UpdateGrade(Guid.NewGuid(), gradeId, grade: 5, attended: null);

        await act.Should().ThrowAsync<Exception>().WithMessage("Оценка не найдена");
    }

    [Fact]
    public async Task UpdateGrade_WhenGradeIsNull_ShouldDeleteGrade()
    {
        var teacherId = Guid.NewGuid();
        var gradeEntity = new StudentGrade { Id = Guid.NewGuid(), Grade = 3 };

        _gradeRepo.Setup(x => x.GetByIdAsync(gradeEntity.Id)).ReturnsAsync(gradeEntity);
        _gradeRepo.Setup(x => x.DeleteAsync(gradeEntity)).Returns(Task.CompletedTask);
        _gradeRepo.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);
        _notificationRepo.Setup(x => x.CreateNotificationAsync(It.IsAny<Notification>()))
            .Returns(Task.CompletedTask);
        SetupParticipation(gradeEntity);

        var result = await CreateService().UpdateGrade(teacherId, gradeEntity.Id, grade: null, attended: null);

        result.Grade.Should().BeNull();
        _gradeRepo.Verify(x => x.DeleteAsync(gradeEntity), Times.Once);
    }

    [Fact]
    public async Task UpdateGrade_WhenAttendedProvided_ShouldSetAttendance()
    {
        var teacherId = Guid.NewGuid();
        var gradeEntity = new StudentGrade { Id = Guid.NewGuid(), Grade = 3 };
        var participation = new LessonParticipation { Attended = false };

        _gradeRepo.Setup(x => x.GetByIdAsync(gradeEntity.Id)).ReturnsAsync(gradeEntity);
        _gradeRepo.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);
        _notificationRepo.Setup(x => x.CreateNotificationAsync(It.IsAny<Notification>()))
            .Returns(Task.CompletedTask);
        SetupParticipation(gradeEntity, existing: participation);

        var result = await CreateService().UpdateGrade(teacherId, gradeEntity.Id, grade: null, attended: true);

        result.Attended.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateGrade_ShouldSendNotification()
    {
        var teacherId = Guid.NewGuid();
        var gradeEntity = new StudentGrade
        {
            Id = Guid.NewGuid(),
            Grade = 3,
            Lesson = new Lesson
            {
                StartsAt = new DateTime(2026, 6, 2),
                Subject = new SubjectEntity { Name = "Математика" }
            }
        };

        _gradeRepo.Setup(x => x.GetByIdAsync(gradeEntity.Id)).ReturnsAsync(gradeEntity);
        _gradeRepo.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);
        _notificationRepo.Setup(x => x.CreateNotificationAsync(It.IsAny<Notification>()))
            .Returns(Task.CompletedTask);
        SetupParticipation(gradeEntity);

        await CreateService().UpdateGrade(teacherId, gradeEntity.Id, grade: 5, attended: null);

        _notificationRepo.Verify(
            x => x.CreateNotificationAsync(It.IsAny<Notification>()),
            Times.Once);
    }
}
}