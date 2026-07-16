using FluentAssertions;
using Moq;
using ProjectScheduleTraining.Application.Students.Commands;
using ProjectScheduleTraining.Application.Students.Handlers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Tests.Unit.Application.Students
{
    /// <summary>
    /// Testes unitários do handler de criação de aluno.
    /// </summary>
    public class CreateStudentHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly CreateStudentHandler _handler;

        public CreateStudentHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.Students).Returns(_studentRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _handler = new CreateStudentHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_WhenValidData_ShouldCreateStudent()
        {
            // Arrange
            var command = new CreateStudentCommand(
                "João Silva",
                "529.982.247-25",
                "joao@email.com",
                "85999999999",
                new DateTime(1990, 1, 1),
                null,
                null);

            _studentRepositoryMock
                .Setup(r => r.CpfExistsAsync(
                    It.IsAny<string>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _studentRepositoryMock
                .Setup(r => r.AddAsync(
                    It.IsAny<Student>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("João Silva");
            result.Email.Should().Be("joao@email.com");

            _studentRepositoryMock.Verify(
                r => r.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                u => u.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WhenCpfAlreadyExists_ShouldThrowDomainException()
        {
            // Arrange
            var command = new CreateStudentCommand(
                "Maria Santos",
                "529.982.247-25",
                "maria@email.com",
                "85988888888",
                new DateTime(1985, 6, 15),
                null,
                null);

            _studentRepositoryMock
                .Setup(r => r.CpfExistsAsync(
                    It.IsAny<string>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*CPF*");

            _studentRepositoryMock.Verify(
                r => r.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                u => u.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
