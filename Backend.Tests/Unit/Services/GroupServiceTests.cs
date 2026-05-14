using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Service;
using Backend.Application.Models.Auth;
using Backend.Application.Models.Group;
using Backend.Application.Services;
using Backend.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Backend.Tests.Unit.Services
{
    public class GroupServiceTests
    {
        public readonly Mock<IGroupRepository> _groupRepositoryMock;
        public readonly IGroupService _groupService;
        public GroupServiceTests()
        {
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _groupService = new GroupService(_groupRepositoryMock.Object);
        }

        [Fact]
        public async Task GetTeacherGroupsAsync_ShouldReturnGroups()
        {
            var userId = Guid.NewGuid();
            var groups = new List<GroupsTeacherDto>
            {
                new GroupsTeacherDto
                (
                    Id: Guid.NewGuid(),
                    Name: "GROUP_21"
                ),
                new GroupsTeacherDto
                (
                    Id: Guid.NewGuid(),
                    Name: "GROUP_22"
                )
            };
            _groupRepositoryMock.Setup(x => x.GetGroupsByTeacherAsync(userId)).ReturnsAsync(groups);
            var service = new GroupService(
                _groupRepositoryMock.Object);
            var result = await service.GetTeacherGroupsAsync(userId);
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetTeacherGroupsAsync_ShouldReturnEmptyList_WhenNoGroups()
        {
            var userId = Guid.NewGuid();
            _groupRepositoryMock.Setup(x => x.GetGroupsByTeacherAsync(userId)).ReturnsAsync(new List<GroupsTeacherDto>());
            var service = new GroupService(
                _groupRepositoryMock.Object);
            var result = await service.GetTeacherGroupsAsync(userId);
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetTeacherGroupsAsync_ShouldReturnEmptyList_WhenTeacherDoesNotExist()
        {
            var userId = Guid.NewGuid();
            _groupRepositoryMock.Setup(x => x.GetGroupsByTeacherAsync(userId)).ReturnsAsync(new List<GroupsTeacherDto>());
            var service = new GroupService(
                _groupRepositoryMock.Object);
            var result = await service.GetTeacherGroupsAsync(userId);
            result.Should().BeEmpty();
        }
    }
}
