using Backend.Application.Interfaces;
using Backend.Application.Models;
using Backend.Application.Services;
using Backend.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Backend.Tests.Unit.Services;

public class NotificationServiceTests
{
    private readonly Mock<INotificationRepository> _notificationRepositoryMock;

    private readonly Mock<IStudentRepository> _studentRepositoryMock;

    private readonly Mock<IGroupRepository> _groupRepositoryMock;

    private readonly Mock<INotificationSender> _notificationSenderMock;

    private readonly NotificationService _service;

    public NotificationServiceTests()
    {
        _notificationRepositoryMock =
            new Mock<INotificationRepository>();

        _studentRepositoryMock =
            new Mock<IStudentRepository>();

        _groupRepositoryMock =
            new Mock<IGroupRepository>();

        _notificationSenderMock =
            new Mock<INotificationSender>();

        _service = new NotificationService(
            _notificationRepositoryMock.Object,
            _studentRepositoryMock.Object,
            _groupRepositoryMock.Object,
            _notificationSenderMock.Object);
    }

    [Fact]
    public async Task GetNotificationsAsync_ShouldReturnNotifications()
    {
        // Arrange

        var userId = Guid.NewGuid();

        var notifications = new List<NotificationResult>
        {
            new NotificationResult(
                Id:Guid.NewGuid(),
                SenderName:"Sender 1",
                SenderLastName:"SenderLastName 1",
                SenderFatherName:"SenderFatherName 1",
                Title:"Title 1",
                Body:"Body 1",
                IsRead: false,
                CreatedAt: DateTime.UtcNow),

            new NotificationResult(
                Id:Guid.NewGuid(),
                SenderName:"Sender 2",
                SenderLastName:"SenderLastName 2",
                SenderFatherName:"SenderFatherName 2",
                Title:"Title 2",
                Body:"Body 2",
                IsRead: false,
                CreatedAt: DateTime.UtcNow)
        };

        _notificationRepositoryMock
            .Setup(x => x.GetNotificationsAsync(userId))
            .ReturnsAsync(notifications);

        var result = await _service
            .GetNotificationsAsync(userId);


        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetNotificationsAsync_ShouldReturnEmpty_WhenNoNotifications()
    {

        var userId = Guid.NewGuid();

        _notificationRepositoryMock
            .Setup(x => x.GetNotificationsAsync(userId))
            .ReturnsAsync(new List<NotificationResult>());

        var result = await _service
            .GetNotificationsAsync(userId);


        result.Should().BeEmpty();
    }
}