using FluentAssertions;
using ProjectScheduleTraining.Application.Plans.Commands;
using ProjectScheduleTraining.Application.Plans.Validators;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Tests.Unit.Validators.Plans
{
    /// <summary>
    /// Testes unitários do validator de criação de plano.
    /// </summary>
    public class CreatePlanCommandValidatorTests
    {
        private readonly CreatePlanCommandValidator _validator;

        public CreatePlanCommandValidatorTests()
        {
            _validator = new CreatePlanCommandValidator();
        }

        [Fact]
        public void Validate_WhenValidData_ShouldNotHaveErrors()
        {
            // Arrange
            var command = new CreatePlanCommand(
                "Plano Mensal 3x",
                PlanType.Monthly,
                WeeklyFrequency.ThreeTimesAWeek,
                1,
                150.00m,
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
            var command = new CreatePlanCommand(
                name,
                PlanType.Monthly,
                WeeklyFrequency.ThreeTimesAWeek,
                1,
                150.00m,
                null);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(CreatePlanCommand.Name));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Validate_WhenPriceIsInvalid_ShouldHaveError(decimal price)
        {
            // Arrange
            var command = new CreatePlanCommand(
                "Plano Mensal 3x",
                PlanType.Monthly,
                WeeklyFrequency.ThreeTimesAWeek,
                1,
                price,
                null);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(CreatePlanCommand.Price));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validate_WhenDurationMonthsIsInvalid_ShouldHaveError(int durationMonths)
        {
            // Arrange
            var command = new CreatePlanCommand(
                "Plano Mensal 3x",
                PlanType.Monthly,
                WeeklyFrequency.ThreeTimesAWeek,
                durationMonths,
                150.00m,
                null);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(CreatePlanCommand.DurationMonths));
        }
    }
}
