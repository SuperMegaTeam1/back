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
        public readonly IJournalService _journalService;
        private readonly Mock<ILessonRepository> _lessonRepo;
        private readonly Mock<IStudentRepository> _studentRepo;
        private readonly Mock<IGradeRepository> _gradeRepo;
        private readonly Mock<IParticipationRepository> _participationRepo;

        public JournalServiceTests()
        {
            _lessonRepo = new Mock<ILessonRepository>();
            _studentRepo = new Mock<IStudentRepository>();
            _gradeRepo = new Mock<IGradeRepository>();
            _participationRepo = new Mock<IParticipationRepository>();
            _journalService = new JournalService(_lessonRepo.Object, _studentRepo.Object, _gradeRepo.Object, _participationRepo.Object);
        }

        [Fact]
        public async Task UpdateJournal_ShouldReturnUpdatedJournal()
        {
            var lessonId = Guid.NewGuid();
            var studentId = Guid.NewGuid(); 

            _studentRepo.Setup(x => x.GetByIdAsync(studentId))
                .ReturnsAsync(new Student { Id = studentId, StudyGroupId = Guid.NewGuid() });

            var request = new UpdateJournalRequest
            {
                Items = new List<JournalItemDto>
        {
            new JournalItemDto
            {
                StudentId = studentId,
                Grade = 5,
                Attended = null
            }
        }
            };

            var result = await _journalService.UpdateJournal(lessonId, request);
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items[0].Grade.Should().Be(5);
        }

        [Fact]
        public async Task UpdateJournal_ShouldThrowException_WhenBothGradeAndAttendedAreNull()
        {
            var lessonId = Guid.NewGuid();
            var request = new UpdateJournalRequest
            {
                Items = new List<JournalItemDto>
                {
                    new JournalItemDto
                    {
                        StudentId = Guid.NewGuid(),
                        Grade = null,
                        Attended = null
                    }
                }
            };
            Func<Task> act = async () => await _journalService.UpdateJournal(lessonId, request);
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Заполните оценку либо поле о посещаемости");
        }
    }
}
