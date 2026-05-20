using FluentAssertions;
using ProjectScheduleTraining.Application.Auth.Commands;
using ProjectScheduleTraining.Application.Auth.Validators;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Tests.Unit.Validators.Auth
{
    /// <summary>
    /// Testes unitários do validator de registro de usuário.
    /// </summary>
    public class RegisterUserCommandValidatorTests
    {
        private readonly RegisterUserCommandValidator _validator;

        public RegisterUserCommandValidatorTests()
        {
            _validator = new RegisterUserCommandValidator();
        }

        [Fact]
        public void Validate_WhenValidData_ShouldNotHaveErrors()
        {
            // Arrange
            var command = new RegisterUserCommand(
                "Administrador",
                "admin@pst.com",
                "Admin@2025",
                UserRole.Admin,
                null);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("12345678")]
        [InlineData("password")]
        [InlineData("PASSWORD1")]
        [InlineData("Password1")]
        public void Validate_WhenPasswordIsWeak_ShouldHaveError(string password)
        {
            // Arrange
            var command = new RegisterUserCommand(
                "Administrador",
                "admin@pst.com",
                password,
                UserRole.Admin,
                null);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(RegisterUserCommand.Password));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Validate_WhenNameIsEmpty_ShouldHaveError(string name)
        {
            // Arrange
            var command = new RegisterUserCommand(
                name,
                "admin@pst.com",
                "Admin@2025",
                UserRole.Admin,
                null);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(RegisterUserCommand.Name));
        }

        [Theory]
        [InlineData("invalidemail")]
        [InlineData("email@")]
        [InlineData("@email.com")]
        public void Validate_WhenEmailIsInvalid_ShouldHaveError(string email)
        {
            // Arrange
            var command = new RegisterUserCommand(
                "Administrador",
                email,
                "Admin@2025",
                UserRole.Admin,
                null);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(RegisterUserCommand.Email));
        }
    }
}
