using FluentAssertions;
using Moq;
using ProjectScheduleTraining.Application.Auth.Commands;
using ProjectScheduleTraining.Application.Auth.Handlers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Tests.Unit.Application.Auth
{
    /// <summary>
    /// Testes unitários do handler de registro de usuário.
    /// </summary>
    public class RegisterUserHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly RegisterUserHandler _handler;

        public RegisterUserHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _handler = new RegisterUserHandler(_unitOfWorkMock.Object, _passwordServiceMock.Object);
        }

        [Fact]
        public async Task Handle_WhenValidData_ShouldRegisterUser()
        {
            // Arrange
            var command = new RegisterUserCommand(
                "Administrador",
                "admin@pst.com",
                "Admin@2025",
                UserRole.Admin,
                null);

            _userRepositoryMock
                .Setup(r => r.EmailExistsAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _passwordServiceMock
                .Setup(p => p.HashPassword(It.IsAny<string>()))
                .Returns("hashedpassword");

            _userRepositoryMock
                .Setup(r => r.AddAsync(
                    It.IsAny<User>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Administrador");
            result.Email.Should().Be("admin@pst.com");
            result.Role.Should().Be(UserRole.Admin);

            _passwordServiceMock.Verify(
                p => p.HashPassword(It.IsAny<string>()),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                u => u.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WhenEmailAlreadyExists_ShouldThrowDomainException()
        {
            // Arrange
            var command = new RegisterUserCommand(
                "Administrador",
                "admin@pst.com",
                "Admin@2025",
                UserRole.Admin,
                null);

            _userRepositoryMock
                .Setup(r => r.EmailExistsAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*e-mail*");

            _userRepositoryMock.Verify(
                r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
