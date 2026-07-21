using FluentAssertions;
using Moq;
using ProjectScheduleTraining.Application.Enrollments.Commands;
using ProjectScheduleTraining.Application.Enrollments.Handlers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Tests.Builders;

namespace ProjectScheduleTraining.Tests.Unit.Application.Enrollments;

/// <summary>
/// Testes unitários do handler de criação de matrícula.
/// </summary>
public class CreateEnrollmentHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly Mock<IPlanRepository> _planRepositoryMock;
    private readonly Mock<IEnrollmentRepository> _enrollmentRepositoryMock;
    private readonly CreateEnrollmentHandler _handler;

    public CreateEnrollmentHandlerTests()
    {
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _planRepositoryMock = new Mock<IPlanRepository>();
        _enrollmentRepositoryMock = new Mock<IEnrollmentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _unitOfWorkMock.Setup(u => u.Students)
            .Returns(_studentRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.Plans)
            .Returns(_planRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.Enrollments)
            .Returns(_enrollmentRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _enrollmentRepositoryMock
            .Setup(r => r.GetByStudentIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Enrollment?)null);

        _handler = new CreateEnrollmentHandler(_unitOfWorkMock.Object);
    }

    /// Comando padrão para reuso nos testes — dia 10, pagamento à vista.
    private static CreateEnrollmentCommand BuildCommand(Guid studentId, Guid planId)
        => new(studentId, planId, PaymentDueDay.Day10, PaymentMethod.Cash);

    [Fact]
    public async Task Handle_WhenValidData_ShouldCreateEnrollment()
    {
        // Arrange
        var student = new StudentBuilder()
            .WithStatus(StudentStatus.Active)
            .Build();

        var plan = new PlanBuilder()
            .WithIsActive(true)
            .Build();

        _studentRepositoryMock
            .Setup(r => r.GetByIdAsync(student.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _planRepositoryMock
            .Setup(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(plan);

        _enrollmentRepositoryMock
            .Setup(r => r.AddAsync(
                It.IsAny<Enrollment>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(
            BuildCommand(student.Id, plan.Id),
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StudentId.Should().Be(student.Id);
        result.PlanId.Should().Be(plan.Id);
        result.IsActive.Should().BeTrue();

        _enrollmentRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Enrollment>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            u => u.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenValidData_ShouldApplyDiscountForLoyaltyPlan()
    {
        // Arrange
        var student = new StudentBuilder()
            .WithStatus(StudentStatus.Active)
            .Build();

        var plan = new PlanBuilder()
            .WithIsActive(true)
            .WithType(PlanType.Annual)
            .WithPrice(396m)
            .Build();

        _studentRepositoryMock
            .Setup(r => r.GetByIdAsync(student.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _planRepositoryMock
            .Setup(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(plan);

        _enrollmentRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Enrollment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        /// Anual à vista = 20% de desconto.
        var command = new CreateEnrollmentCommand(
            student.Id, plan.Id, PaymentDueDay.Day10, PaymentMethod.Cash);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.DiscountPercentage.Should().Be(20m);
        result.FinalPrice.Should().Be(316.80m);
    }

    [Fact]
    public async Task Handle_WhenStudentNotFound_ShouldThrowDomainException()
    {
        // Arrange
        _studentRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        // Act
        var act = async () => await _handler.Handle(
            BuildCommand(Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("*não encontrado*");
    }

    [Fact]
    public async Task Handle_WhenStudentIsInactive_ShouldThrowDomainException()
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
            BuildCommand(student.Id, Guid.NewGuid()),
            CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("*inativo*");

        _unitOfWorkMock.Verify(
            u => u.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenStudentIsBlocked_ShouldThrowDomainException()
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
            BuildCommand(student.Id, Guid.NewGuid()),
            CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("*bloqueado*");

        _unitOfWorkMock.Verify(
            u => u.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPlanNotFound_ShouldThrowDomainException()
    {
        // Arrange
        var student = new StudentBuilder()
            .WithStatus(StudentStatus.Active)
            .Build();

        _studentRepositoryMock
            .Setup(r => r.GetByIdAsync(student.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _planRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Plan?)null);

        // Act
        var act = async () => await _handler.Handle(
            BuildCommand(student.Id, Guid.NewGuid()),
            CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("*não encontrado*");
    }

    [Fact]
    public async Task Handle_WhenPlanIsInactive_ShouldThrowDomainException()
    {
        // Arrange
        var student = new StudentBuilder()
            .WithStatus(StudentStatus.Active)
            .Build();

        var plan = new PlanBuilder()
            .WithIsActive(false)
            .Build();

        _studentRepositoryMock
            .Setup(r => r.GetByIdAsync(student.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _planRepositoryMock
            .Setup(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(plan);

        // Act
        var act = async () => await _handler.Handle(
            BuildCommand(student.Id, plan.Id),
            CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("*inativo*");
    }

    [Fact]
    public async Task Handle_WhenStudentAlreadyHasActiveEnrollment_ShouldThrowDomainException()
    {
        // Arrange
        var student = new StudentBuilder()
            .WithStatus(StudentStatus.Active)
            .Build();

        var plan = new PlanBuilder()
            .WithIsActive(true)
            .Build();

        var existingEnrollment = new Enrollment
        {
            StudentId = student.Id,
            PlanId = plan.Id,
            StartDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddMonths(1),
            PaymentDueDay = PaymentDueDay.Day10,
            PaymentMethod = PaymentMethod.Cash,
            IsActive = true
        };

        _studentRepositoryMock
            .Setup(r => r.GetByIdAsync(student.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _planRepositoryMock
            .Setup(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(plan);

        _enrollmentRepositoryMock
            .Setup(r => r.GetByStudentIdAsync(student.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEnrollment);

        // Act
        var act = async () => await _handler.Handle(
            BuildCommand(student.Id, plan.Id),
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