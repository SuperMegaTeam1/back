using Backend.Application.Interfaces;
using Backend.Application.Services;
using Backend.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Backend.Tests.Unit.Services
{
    public class TeachersSubjectsServiceTests
    {
        public readonly Mock<ISubjectRepository> _subjectsRepoMock;

        public TeachersSubjectsServiceTests()
        {
            _subjectsRepoMock = new Mock<ISubjectRepository>();
        }

        [Fact]
        public async Task GetSubjectsForTeacherAsync_ShouldGroupSubjects()
        {
            var subjectId = Guid.NewGuid();

            var data = new List<(Guid, string, Guid, string)>
            {
                (
                    subjectId,
                    "Math",
                    Guid.NewGuid(),
                    "GROUP_21"
                ),

                (
                    subjectId,
                    "Math",
                    Guid.NewGuid(),
                    "GROUP_22"
                )
            };

            _subjectsRepoMock
                .Setup(x => x.GetSubjectsByTeacherIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(data);

            var service = new TeachersSubjectsService(
                _subjectsRepoMock.Object);

            var result = await service
                .GetSubjectsForTeacherAsync(Guid.NewGuid());

            result.Should().HaveCount(1);

            result.First().StudyGroups
                .Should().HaveCount(2);
        }
    }
}