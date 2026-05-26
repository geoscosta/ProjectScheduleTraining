using FluentAssertions;
using Moq;
using ProjectScheduleTraining.Application.Students.Commands;
using ProjectScheduleTraining.Application.Students.Handlers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Tests.Builders;

namespace ProjectScheduleTraining.Tests.Unit.Application.Students
{
    /// <summary>
    /// Testes unitários do handler de bloqueio de aluno.
    /// </summary>
    public class BlockStudentHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IEnrollmentRepository> _enrollmentRepositoryMock;
        private readonly BlockStudentHandler _handler;

        public BlockStudentHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _enrollmentRepositoryMock = new Mock<IEnrollmentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(u => u.Students)
                .Returns(_studentRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Enrollments)
                .Returns(_enrollmentRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            /// Configura o mock padrão de enrollment como null
            /// para não interferir nos demais testes.
            _enrollmentRepositoryMock
                .Setup(r => r.GetByStudentIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Enrollment?)null);

            _handler = new BlockStudentHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_WhenStudentIsActive_ShouldBlockStudent()
        {
            // Arrange
            var student = new StudentBuilder()
                .WithStatus(StudentStatus.Active)
                .Build();

            _studentRepositoryMock
                .Setup(r => r.GetByIdAsync(student.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(student);

            // Act
            await _handler.Handle(new BlockStudentCommand(student.Id), CancellationToken.None);

            // Assert
            student.Status.Should().Be(StudentStatus.Blocked);

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
                new BlockStudentCommand(Guid.NewGuid()),
                CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*não encontrado*");
        }

        [Fact]
        public async Task Handle_WhenStudentAlreadyBlocked_ShouldThrowDomainException()
        {
            // Arrange
            var student = new StudentBuilder()
                .WithStatus(StudentStatus.Blocked)
                .Build();

            _studentRepositoryMock
                .Setup(r => r.GetByIdAsync(student.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(student);

            // Act
            var act = async () => await _handler.Handle(
                new BlockStudentCommand(student.Id),
                CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*bloqueado*");
        }

        [Fact]
        public async Task Handle_WhenStudentHasActiveEnrollment_ShouldThrowDomainException()
        {
            // Arrange
            var student = new StudentBuilder()
                .WithStatus(StudentStatus.Active)
                .Build();

            var enrollment = new Enrollment
            {
                StudentId = student.Id,
                PlanId = Guid.NewGuid(),
                StartDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMonths(1),
                PaymentDueDay = 10,
                IsActive = true
            };

            _studentRepositoryMock
                .Setup(r => r.GetByIdAsync(student.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(student);

            _enrollmentRepositoryMock
                .Setup(r => r.GetByStudentIdAsync(student.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(enrollment);

            // Act
            var act = async () => await _handler.Handle(
                new BlockStudentCommand(student.Id),
                CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*matrícula ativa*");

            _unitOfWorkMock.Verify(
                u => u.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
