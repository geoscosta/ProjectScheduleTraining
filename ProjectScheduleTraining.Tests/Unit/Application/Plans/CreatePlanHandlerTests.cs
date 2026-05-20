using FluentAssertions;
using Moq;
using ProjectScheduleTraining.Application.Plans.Commands;
using ProjectScheduleTraining.Application.Plans.Handlers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Tests.Builders;

namespace ProjectScheduleTraining.Tests.Unit.Application.Plans
{
    /// <summary>
    /// Testes unitários do handler de criação de plano.
    /// </summary>
    public class CreatePlanHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPlanRepository> _planRepositoryMock;
        private readonly CreatePlanHandler _handler;

        public CreatePlanHandlerTests()
        {
            _planRepositoryMock = new Mock<IPlanRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.Plans).Returns(_planRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _handler = new CreatePlanHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_WhenValidData_ShouldCreatePlan()
        {
            // Arrange
            var command = new CreatePlanCommand(
                "Plano Mensal 3x",
                PlanType.Monthly,
                WeeklyFrequency.ThreeTimesAWeek,
                1,
                150.00m,
                "Plano mensal com 3 aulas por semana");

            _planRepositoryMock
                .Setup(r => r.GetActivePlansAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Plan>());

            _planRepositoryMock
                .Setup(r => r.AddAsync(
                    It.IsAny<Plan>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Plano Mensal 3x");
            result.Price.Should().Be(150.00m);
            result.IsActive.Should().BeTrue();

            _planRepositoryMock.Verify(
                r => r.AddAsync(It.IsAny<Plan>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                u => u.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WhenPlanNameAlreadyExists_ShouldThrowDomainException()
        {
            // Arrange
            var existingPlan = new PlanBuilder()
                .WithName("Plano Mensal 3x")
                .Build();

            var command = new CreatePlanCommand(
                "Plano Mensal 3x",
                PlanType.Monthly,
                WeeklyFrequency.ThreeTimesAWeek,
                1,
                150.00m,
                null);

            _planRepositoryMock
                .Setup(r => r.GetActivePlansAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Plan> { existingPlan });

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*nome*");

            _planRepositoryMock.Verify(
                r => r.AddAsync(It.IsAny<Plan>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
