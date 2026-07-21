using FluentAssertions;
using ProjectScheduleTraining.Application.Students.Commands;
using ProjectScheduleTraining.Application.Students.Validators;

namespace ProjectScheduleTraining.Tests.Unit.Validators.Students;

/// <summary>
/// Testes unitários do validator de criação de aluno.
/// </summary>
public class CreateStudentCommandValidatorTests
{
    private readonly CreateStudentCommandValidator _validator;

    public CreateStudentCommandValidatorTests()
    {
        _validator = new CreateStudentCommandValidator();
    }

    /// Command padrão válido para reuso nos testes.
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
    public void Validate_WhenValidData_ShouldNotHaveErrors()
    {
        // Arrange
        var command = BuildCommand();

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WhenNameIsEmpty_ShouldHaveError(string name)
    {
        // Arrange
        var command = BuildCommand(name: name);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == nameof(CreateStudentCommand.Name));
    }

    [Theory]
    [InlineData("12345678900")]
    [InlineData("00000000000")]
    [InlineData("111")]
    public void Validate_WhenCpfIsInvalid_ShouldHaveError(string cpf)
    {
        // Arrange
        var command = BuildCommand(cpf: cpf);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == nameof(CreateStudentCommand.Cpf));
    }

    [Theory]
    [InlineData("invalidemail")]
    [InlineData("email@")]
    [InlineData("@email.com")]
    public void Validate_WhenEmailIsInvalid_ShouldHaveError(string email)
    {
        // Arrange
        var command = BuildCommand(email: email);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == nameof(CreateStudentCommand.Email));
    }

    [Fact]
    public void Validate_WhenBirthDateIsInFuture_ShouldHaveError()
    {
        // Arrange
        var command = BuildCommand(birthDate: DateTime.Today.AddDays(1));

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == nameof(CreateStudentCommand.BirthDate));
    }

    [Fact]
    public void Validate_WhenMinorWithoutGuardian_ShouldHaveError()
    {
        // Arrange
        var command = BuildCommand(birthDate: DateTime.Today.AddYears(-15));

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == nameof(CreateStudentCommand.GuardianName));
    }

    [Fact]
    public void Validate_WhenStateHasMoreThanTwoChars_ShouldHaveError()
    {
        // Arrange
        var command = new CreateStudentCommand(
            "João Silva",
            "529.982.247-25",
            "joao@email.com",
            "85999999999",
            new DateTime(1990, 1, 1),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            "Ceará",
            null,
            null,
            null,
            null,
            false,
            false);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == nameof(CreateStudentCommand.State));
    }
}