using Backend.Application.Interfaces;
using Backend.Application.Models.Auth;
using Backend.Application.Models.Journal;
using Backend.Application.Services;
using Backend.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Backend.Tests.Unit.Services
{
    public class JournalServiceTests
    {
        private readonly Mock<ILessonRepository> _lessonRepo;
        private readonly Mock<IStudentRepository> _studentRepo;
        private readonly Mock<IGradeRepository> _gradeRepo;
        private readonly Mock<IParticipationRepository> _participationRepo;
        private readonly Mock<INotificationSender> _notificationSender;
        private readonly Mock<INotificationRepository> _notificationRepo;
        private readonly IJournalService _journalService;

        public JournalServiceTests()
        {
            _lessonRepo = new Mock<ILessonRepository>();
            _studentRepo = new Mock<IStudentRepository>();
            _gradeRepo = new Mock<IGradeRepository>();
            _participationRepo = new Mock<IParticipationRepository>();
            _notificationSender = new Mock<INotificationSender>(); // ✅ до создания сервиса
            _notificationRepo = new Mock<INotificationRepository>(); // ✅ до создания сервиса

            _journalService = new JournalService(
                _lessonRepo.Object,
                _studentRepo.Object,
                _gradeRepo.Object,
                _participationRepo.Object,
                _notificationSender.Object,
                _notificationRepo.Object);
        }

        [Fact]
        public async Task UpdateJournal_ShouldReturnUpdatedJournal()
        {
            var userId = Guid.NewGuid();
            var lessonId = Guid.NewGuid();
            var studentId = Guid.NewGuid();

            _studentRepo.Setup(x => x.GetByIdAsync(studentId))
                .ReturnsAsync(new Student { Id = studentId, StudyGroupId = Guid.NewGuid() });

            _gradeRepo.Setup(x => x.GetByStudentLesson(studentId, lessonId))
                .ReturnsAsync((StudentGrade)null);
            _gradeRepo.Setup(x => x.AddAsync(It.IsAny<StudentGrade>()))
                .Returns(Task.CompletedTask);
            _gradeRepo.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            _participationRepo.Setup(x => x.Get(studentId, lessonId))
                .ReturnsAsync((LessonParticipation)null);
            _participationRepo.Setup(x => x.AddAsync(It.IsAny<LessonParticipation>()))
                .Returns(Task.CompletedTask);
            _participationRepo.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            _notificationRepo.Setup(x => x.CreateNotificationAsync(It.IsAny<Notification>()))
                .Returns(Task.CompletedTask);
            _notificationSender.Setup(x => x.SendNotificationAsync(It.IsAny<Notification>()))
                .Returns(Task.CompletedTask);

            var request = new UpdateJournalRequest
            {
                Items = new List<JournalItemDto>
                {
                    new JournalItemDto { StudentId = studentId, Grade = 5, Attended = null }
                }
            };

            var result = await _journalService.UpdateJournal(userId, lessonId, request); // ✅ userId добавлен
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items[0].Grade.Should().Be(5);
        }

        [Fact]
        public async Task UpdateJournal_ShouldThrowException_WhenBothGradeAndAttendedAreNull()
        {
            var userId = Guid.NewGuid();
            var lessonId = Guid.NewGuid();

            var request = new UpdateJournalRequest
            {
                Items = new List<JournalItemDto>
                {
                    new JournalItemDto { StudentId = Guid.NewGuid(), Grade = null, Attended = null }
                }
            };

            Func<Task> act = async () => await _journalService.UpdateJournal(userId, lessonId, request);
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Заполните оценку либо поле о посещаемости");
        }

        [Fact]
        public async Task UpdateJournal_ShouldThrowException_WhenBothGradeAndAttendedAreFilled()
        {
            var userId = Guid.NewGuid();
            var lessonId = Guid.NewGuid();

            var request = new UpdateJournalRequest
            {
                Items = new List<JournalItemDto>
                {
                    new JournalItemDto { StudentId = Guid.NewGuid(), Grade = 5, Attended = true }
                }
            };

            Func<Task> act = async () => await _journalService.UpdateJournal(userId, lessonId, request);
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Заполните оценку либо поле о посещаемости");
        }

        [Fact]
        public async Task UpdateJournal_ShouldThrowException_WhenStudentNotFound()
        {
            var userId = Guid.NewGuid();
            var lessonId = Guid.NewGuid();
            var studentId = Guid.NewGuid();

            _studentRepo.Setup(x => x.GetByIdAsync(studentId))
                .ReturnsAsync((Student)null);

            var request = new UpdateJournalRequest
            {
                Items = new List<JournalItemDto>
                {
                    new JournalItemDto { StudentId = studentId, Grade = 5, Attended = null }
                }
            };

            Func<Task> act = async () => await _journalService.UpdateJournal(userId, lessonId, request);
            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"Студент {studentId} не найден");
        }
    }
}
