using FluentAssertions;
using ProjectScheduleTraining.Application.Students.Commands;
using ProjectScheduleTraining.Application.Students.Validators;

namespace ProjectScheduleTraining.Tests.Unit.Validators.Students
{
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

        [Fact]
        public void Validate_WhenValidData_ShouldNotHaveErrors()
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
            var command = new CreateStudentCommand(
                name,
                "529.982.247-25",
                "joao@email.com",
                "85999999999",
                new DateTime(1990, 1, 1),
                null,
                null);

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
            var command = new CreateStudentCommand(
                "João Silva",
                cpf,
                "joao@email.com",
                "85999999999",
                new DateTime(1990, 1, 1),
                null,
                null);

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
            var command = new CreateStudentCommand(
                "João Silva",
                "529.982.247-25",
                email,
                "85999999999",
                new DateTime(1990, 1, 1),
                null,
                null);

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
            var command = new CreateStudentCommand(
                "João Silva",
                "529.982.247-25",
                "joao@email.com",
                "85999999999",
                DateTime.Today.AddDays(1),
                null,
                null);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(CreateStudentCommand.BirthDate));
        }
    }
}
