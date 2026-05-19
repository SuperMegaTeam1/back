using Backend.Application.Interfaces;
using Backend.Application.Models.Auth;
using Backend.Application.Services;
using Backend.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Backend.Tests.Unit.Services
{
    public class GradeServiceTests
    {
        public readonly Mock<IGradeRepository> _gradeRepo;
        public readonly IGradeService _gradeService;

        public GradeServiceTests()
        {
            _gradeRepo = new Mock<IGradeRepository>();
            _gradeService = new GradeService(_gradeRepo.Object);
        }

        [Fact]
        public async Task UpdateGrade_ShouldReturnUpdatedGrade()
        {
            var gradeEntity = new StudentGrade
            {
                Id = Guid.NewGuid(),
                Grade = 3
            };

            _gradeRepo
                .Setup(x => x.GetByIdAsync(gradeEntity.Id))
                .ReturnsAsync(gradeEntity);

            _gradeRepo
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var service = new GradeService(_gradeRepo.Object);
            var result = await service.UpdateGrade(gradeEntity.Id, 5);
            result.Grade.Should().Be(5);
            _gradeRepo.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateGrade_ShouldThrowException_WhenGradeNotFound()
        {
            var gradeId = Guid.NewGuid();
            _gradeRepo
                .Setup(x => x.GetByIdAsync(gradeId))
                .ReturnsAsync((StudentGrade)null);

            var service = new GradeService(_gradeRepo.Object);

            Func<Task> act = async () => await service.UpdateGrade(gradeId, 5);
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Оценка не найдена");
        }

        [Fact]
        public async Task UpdateGrade_ShouldCallSaveChangesAsync()
        {
            var gradeEntity = new StudentGrade
            {
                Id = Guid.NewGuid(),
                Grade = 3
            };
            _gradeRepo
                .Setup(x => x.GetByIdAsync(gradeEntity.Id))
                .ReturnsAsync(gradeEntity);
            _gradeRepo
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);
            var service = new GradeService(_gradeRepo.Object);
            await service.UpdateGrade(gradeEntity.Id, 5);
            _gradeRepo.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }
    }
}
