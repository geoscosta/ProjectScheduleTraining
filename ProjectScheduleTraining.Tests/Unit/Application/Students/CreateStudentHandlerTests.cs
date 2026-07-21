using FluentAssertions;
using Moq;
using ProjectScheduleTraining.Application.Students.Commands;
using ProjectScheduleTraining.Application.Students.Handlers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Tests.Unit.Application.Students;

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

    /// Command padrão para reuso nos testes — dados mínimos válidos.
    private static CreateStudentCommand BuildCommand(
        string name = "João Silva",
        string cpf = "529.982.247-25",
        string email = "joao@email.com",
        string phone = "85999999999",
        DateTime? birthDate = null)
        => new(
            name,
            cpf,
            email,
            phone,
            birthDate ?? new DateTime(1990, 1, 1),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            false,
            false);

    [Fact]
    public async Task Handle_WhenValidData_ShouldCreateStudent()
    {
        // Arrange
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
        var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

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
        _studentRepositoryMock
            .Setup(r => r.CpfExistsAsync(
                It.IsAny<string>(),
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _handler.Handle(
            BuildCommand(name: "Maria Santos", cpf: "529.982.247-25", email: "maria@email.com"),
            CancellationToken.None);

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

    [Fact]
    public async Task Handle_WhenMinorWithoutGuardian_ShouldThrowDomainException()
    {
        // Arrange
        _studentRepositoryMock
            .Setup(r => r.CpfExistsAsync(
                It.IsAny<string>(),
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        /// Aluno menor de idade sem responsável deve lançar exceção.
        var command = BuildCommand(birthDate: DateTime.Today.AddYears(-15));

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("*responsável*");
    }
}