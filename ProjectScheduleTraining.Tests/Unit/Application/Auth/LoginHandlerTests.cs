using FluentAssertions;
using Moq;
using ProjectScheduleTraining.Application.Auth.Commands;
using ProjectScheduleTraining.Application.Auth.Handlers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Tests.Builders;

namespace ProjectScheduleTraining.Tests.Unit.Application.Auth
{
    /// <summary>
    /// Testes unitários do handler de login.
    /// </summary>
    public class LoginHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly LoginHandler _handler;

        public LoginHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _authServiceMock = new Mock<IAuthService>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.RefreshTokens).Returns(_refreshTokenRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _handler = new LoginHandler(
                _unitOfWorkMock.Object,
                _authServiceMock.Object,
                _passwordServiceMock.Object);
        }

        [Fact]
        public async Task Handle_WhenValidCredentials_ShouldReturnAuthResponse()
        {
            // Arrange
            var user = new UserBuilder()
                .WithEmail("admin@pst.com")
                .WithPasswordHash("hashedpassword")
                .WithIsActive(true)
                .Build();

            var command = new LoginCommand("admin@pst.com", "Admin@2025");

            _userRepositoryMock
                .Setup(r => r.GetByEmailWithTokensAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordServiceMock
                .Setup(p => p.VerifyPassword(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(true);

            _authServiceMock
                .Setup(a => a.GenerateAccessToken(It.IsAny<User>()))
                .Returns("access_token");

            _authServiceMock
                .Setup(a => a.GenerateRefreshToken())
                .Returns("refresh_token");

            _authServiceMock
                .Setup(a => a.GetAccessTokenExpirationMinutes())
                .Returns(15);

            _authServiceMock
                .Setup(a => a.GetRefreshTokenExpirationDays())
                .Returns(7);

            _refreshTokenRepositoryMock
                .Setup(r => r.AddAsync(
                    It.IsAny<RefreshToken>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be("access_token");
            result.RefreshToken.Should().Be("refresh_token");
            result.User.Email.Should().Be(user.Email);

            _refreshTokenRepositoryMock.Verify(
                r => r.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                u => u.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ShouldThrowDomainException()
        {
            // Arrange
            var command = new LoginCommand("notfound@pst.com", "Admin@2025");

            _userRepositoryMock
                .Setup(r => r.GetByEmailWithTokensAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*inválidos*");
        }

        [Fact]
        public async Task Handle_WhenInvalidPassword_ShouldThrowDomainException()
        {
            // Arrange
            var user = new UserBuilder()
                .WithEmail("admin@pst.com")
                .WithPasswordHash("hashedpassword")
                .Build();

            var command = new LoginCommand("admin@pst.com", "WrongPassword");

            _userRepositoryMock
                .Setup(r => r.GetByEmailWithTokensAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordServiceMock
                .Setup(p => p.VerifyPassword(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(false);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*inválidos*");
        }

        [Fact]
        public async Task Handle_WhenUserIsInactive_ShouldThrowDomainException()
        {
            // Arrange
            var user = new UserBuilder()
                .WithEmail("admin@pst.com")
                .WithPasswordHash("hashedpassword")
                .WithIsActive(false)
                .Build();

            var command = new LoginCommand("admin@pst.com", "Admin@2025");

            _userRepositoryMock
                .Setup(r => r.GetByEmailWithTokensAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordServiceMock
                .Setup(p => p.VerifyPassword(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(true);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*inativo*");
        }
    }
}
