using FluentAssertions;
using Moq;
using ProjectScheduleTraining.Application.Students.Commands;
using ProjectScheduleTraining.Application.Students.Handlers;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Tests.Builders;

namespace ProjectScheduleTraining.Tests.Unit.Application.Students
{
    /// <summary>
    /// Testes unitários do handler de inativação de aluno.
    /// </summary>
    public class DeactivateStudentHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly DeactivateStudentHandler _handler;

        public DeactivateStudentHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.Students).Returns(_studentRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _handler = new DeactivateStudentHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_WhenStudentIsActive_ShouldDeactivateStudent()
        {
            // Arrange
            var student = new StudentBuilder()
                .WithStatus(StudentStatus.Active)
                .Build();

            _studentRepositoryMock
                .Setup(r => r.GetByIdAsync(student.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(student);

            // Act
            await _handler.Handle(
                new DeactivateStudentCommand(student.Id),
                CancellationToken.None);

            // Assert
            student.Status.Should().Be(StudentStatus.Inactive);

            _unitOfWorkMock.Verify(
                u => u.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WhenStudentNotFound_ShouldThrowDomainException()
        {
            // Arrange
            _studentRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Student?)null);

            // Act
            var act = async () => await _handler.Handle(
                new DeactivateStudentCommand(Guid.NewGuid()),
                CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*não encontrado*");
        }

        [Fact]
        public async Task Handle_WhenStudentAlreadyInactive_ShouldThrowDomainException()
        {
            // Arrange
            var student = new StudentBuilder()
                .WithStatus(StudentStatus.Inactive)
                .Build();

            _studentRepositoryMock
                .Setup(r => r.GetByIdAsync(student.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(student);

            // Act
            var act = async () => await _handler.Handle(
                new DeactivateStudentCommand(student.Id),
                CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*inativo*");
        }
    }
}
