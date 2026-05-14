using Backend.Application.Interfaces;
using Backend.Application.Models.Auth;
using Backend.Application.Services;
using Backend.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Backend.Tests.Unit.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IAuthRepository> _authRepoMock;

        private readonly Mock<ITokenService> _tokenServiceMock;

        public AuthServiceTests()
        {
            _authRepoMock = new Mock<IAuthRepository>();

            _tokenServiceMock = new Mock<ITokenService>();
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnAuthResult_WhenCredentialsAreValid()
        {

            var user = new AuthUser
           (
               Id: Guid.NewGuid(),
               RoleName: "Student",
               FirstName: "Ivan",
               LastName: "Ivanov",
               FatherName: "Ivanovich",
               Email: "test@test.com",
               StudentId: Guid.NewGuid(),
               TeacherId: null,
               GroupId: Guid.NewGuid(),
               GroupName: "Group 1"
           );

            _authRepoMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _authRepoMock
                .Setup(x => x.CheckPasswordAsync(user.Id, "1234"))
                .ReturnsAsync(true);

            _tokenServiceMock
                .Setup(x => x.CreateToken(user))
                .Returns("JWT_TOKEN");

            var service = new AuthService(
                _authRepoMock.Object,
                _tokenServiceMock.Object);


            var result = await service.LoginAsync(
                user.Email,
                "1234");


            result.Should().NotBeNull();

            result!.Token.Should().Be("JWT_TOKEN");

            result.User.Email.Should().Be(user.Email);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenUserNotFound()
        {
            _authRepoMock
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((AuthUser?)null);

            var service = new AuthService(
                _authRepoMock.Object,
                _tokenServiceMock.Object);

            var result = await service.LoginAsync(
                "test@test.com",
                "1234");

            result.Should().BeNull();
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenPasswordInvalid()
        {
            var user = new AuthUser
            (
                Id: Guid.NewGuid(),
                RoleName: "Student",
                FirstName: "Ivan",
                LastName: "Ivanov",
                FatherName: "Ivanovich",
                Email: "test@test.com",
                StudentId: Guid.NewGuid(),
                TeacherId: null,
                GroupId: Guid.NewGuid(),
                GroupName: "Group 1"
            );

            var authRepoMock = new Mock<IAuthRepository>();
            var tokenServiceMock = new Mock<ITokenService>();

            _authRepoMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _authRepoMock
                .Setup(x => x.CheckPasswordAsync(user.Id, "wrong"))
                .ReturnsAsync(false);

            var service = new AuthService(
                _authRepoMock.Object,
                _tokenServiceMock.Object);

            var result = await service.LoginAsync(
                user.Email,
                "wrong");

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser()
        {

        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenUserNotFound()
        {
            _authRepoMock
                .Setup(x => x.FindByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((AuthUser?)null);
            var service = new AuthService(
                _authRepoMock.Object,
                _tokenServiceMock.Object);
            var result = await service.GetByIdAsync(Guid.NewGuid());
            result.Should().BeNull();
        }
    }
}
