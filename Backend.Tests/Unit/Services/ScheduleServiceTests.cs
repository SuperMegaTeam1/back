using Backend.Application.Interfaces;
using Backend.Application.Models.Shedule;
using Backend.Application.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Unit.Services
{
    public class ScheduleServiceTests
    {
        private readonly Mock<IScheduleRepository> _scheduleRepo;
        private readonly ScheduleService _scheduleService;

        public ScheduleServiceTests()
        {
            _scheduleRepo = new Mock<IScheduleRepository>();
            _scheduleService = new ScheduleService(_scheduleRepo.Object);
        }


        [Fact]
        public async Task GetWeekScheduleAsync_WithSpecificDate_ShouldCalculateCorrectWeekRange()
        {
            var userId = Guid.NewGuid();
            var inputDate = new DateOnly(2026, 5, 14);
            var expectedMonday = new DateOnly(2026, 5, 11);
            var expectedSaturday = new DateOnly(2026, 5, 16);

            var repositoryResult = new List<TodayScheduleResult>
            {
                new("2026-05-11", "Monday", 20, 3, new List<ScheduleLessonsResult>()),
                new("2026-05-14", "Thursday", 20, 1, new List<ScheduleLessonsResult>())
            };

            _scheduleRepo
                .Setup(x => x.GetScheduleAsync(userId, expectedMonday, expectedSaturday))
                .ReturnsAsync(repositoryResult);

            var result = await _scheduleService.GetWeekScheduleAsync(userId, inputDate);

            result.DateStart.Should().Be("2026-05-11");
            result.DateEnd.Should().Be("2026-05-16");
            result.Items.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetWeekScheduleAsync_WithMonday_ShouldReturnSameDayAsStart()
        {
            var userId = Guid.NewGuid();
            var monday = new DateOnly(2026, 5, 11);
            var expectedSaturday = new DateOnly(2026, 5, 16);

            _scheduleRepo
                .Setup(x => x.GetScheduleAsync(userId, monday, expectedSaturday))
                .ReturnsAsync(new List<TodayScheduleResult>());

            var result = await _scheduleService.GetWeekScheduleAsync(userId, monday);

            result.DateStart.Should().Be("2026-05-11");
            result.DateEnd.Should().Be("2026-05-16");
        }

        [Fact]
        public async Task GetWeekScheduleAsync_WithSunday_ShouldTreatAsNextWeek()
        {
            var userId = Guid.NewGuid();
            var sunday = new DateOnly(2026, 5, 17);
            var expectedMonday = new DateOnly(2026, 5, 11);
            var expectedSaturday = new DateOnly(2026, 5, 16);

            _scheduleRepo
                .Setup(x => x.GetScheduleAsync(userId, expectedMonday, expectedSaturday))
                .ReturnsAsync(new List<TodayScheduleResult>());

            var result = await _scheduleService.GetWeekScheduleAsync(userId, sunday);

            result.DateStart.Should().Be("2026-05-11");
            result.DateEnd.Should().Be("2026-05-16");
        }

        [Fact]
        public async Task GetWeekScheduleAsync_WithoutDate_ShouldUseCurrentDate()
        {
            var userId = Guid.NewGuid();
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var expectedMonday = today.AddDays(-1 * diff);
            var expectedSaturday = expectedMonday.AddDays(5);

            _scheduleRepo
                .Setup(x => x.GetScheduleAsync(userId, expectedMonday, expectedSaturday))
                .ReturnsAsync(new List<TodayScheduleResult>());

            var result = await _scheduleService.GetWeekScheduleAsync(userId, null);

            result.DateStart.Should().Be(expectedMonday.ToString("yyyy-MM-dd"));
            result.DateEnd.Should().Be(expectedSaturday.ToString("yyyy-MM-dd"));
        }

        [Fact]
        public async Task GetWeekScheduleAsync_WithEmptyRepositoryResult_ShouldReturnEmptyItems()
        {
            var userId = Guid.NewGuid();
            var date = new DateOnly(2026, 5, 14);
            var expectedMonday = new DateOnly(2026, 5, 11);
            var expectedSaturday = new DateOnly(2026, 5, 16);

            _scheduleRepo
                .Setup(x => x.GetScheduleAsync(userId, expectedMonday, expectedSaturday))
                .ReturnsAsync(new List<TodayScheduleResult>());

            var result = await _scheduleService.GetWeekScheduleAsync(userId, date);

            result.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task GetTodayScheduleAsync_WithSpecificDate_ShouldReturnScheduleForThatDay()
        {
            var userId = Guid.NewGuid();
            var date = new DateOnly(2026, 5, 14);

            var repositoryResult = new List<TodayScheduleResult>
            {
                new("2026-05-14", "Thursday", 20, 3, new List<ScheduleLessonsResult>
                {
                    new(Guid.NewGuid(), Guid.NewGuid(), "Math", Guid.NewGuid(),
                        "John", "Doe", null, Guid.NewGuid(), "Group A",
                        "101", "Lecture", null, "09:00", "10:30")
                })
            };

            _scheduleRepo
                .Setup(x => x.GetScheduleAsync(userId, date, date))
                .ReturnsAsync(repositoryResult);

            var result = await _scheduleService.GetTodayScheduleAsync(userId, date);

            result.Should().NotBeNull();
            result.Date.Should().Be("2026-05-14");
            result.DayName.Should().Be("Thursday");
            result.Items.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetTodayScheduleAsync_WithoutDate_ShouldUseCurrentDate()
        {
            var userId = Guid.NewGuid();
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            _scheduleRepo
                .Setup(x => x.GetScheduleAsync(userId, today, today))
                .ReturnsAsync(new List<TodayScheduleResult>());

            var result = await _scheduleService.GetTodayScheduleAsync(userId, null);

            result.Date.Should().Be(today.ToString("yyyy-MM-dd"));
        }


        [Fact]
        public async Task GetTodayScheduleAsync_WithMultipleLessons_ShouldReturnAllLessons()
        {
            var userId = Guid.NewGuid();
            var date = new DateOnly(2026, 5, 14);

            var lessons = new List<ScheduleLessonsResult>
            {
                new(Guid.NewGuid(), Guid.NewGuid(), "Math", Guid.NewGuid(),
                    "John", "Doe", null, Guid.NewGuid(), "Group A",
                    "101", "Lecture", null, "09:00", "10:30"),
                new(Guid.NewGuid(), Guid.NewGuid(), "Physics", Guid.NewGuid(),
                    "Jane", "Smith", null, Guid.NewGuid(), "Group A",
                    "202", "Lab", null, "10:45", "12:15"),
                new(Guid.NewGuid(), Guid.NewGuid(), "History", Guid.NewGuid(),
                    "Bob", "Johnson", null, Guid.NewGuid(), "Group B",
                    "305", "Seminar", null, "13:00", "14:30")
            };

            var repositoryResult = new List<TodayScheduleResult>
            {
                new("2026-05-14", "Thursday", 20, 3, lessons)
            };

            _scheduleRepo
                .Setup(x => x.GetScheduleAsync(userId, date, date))
                .ReturnsAsync(repositoryResult);

            var result = await _scheduleService.GetTodayScheduleAsync(userId, date);

            result.Items.Should().HaveCount(3);
            result.LessonsWeek.Should().Be(3);
            result.Items.Select(x => x.SubjectName).Should().BeEquivalentTo(
                new[] { "Math", "Physics", "History" }
            );
        }
    }
}