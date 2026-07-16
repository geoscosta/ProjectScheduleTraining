using FluentAssertions;
using Moq;
using ProjectScheduleTraining.Application.Plans.Commands;
using ProjectScheduleTraining.Application.Plans.Handlers;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Tests.Builders;

namespace ProjectScheduleTraining.Tests.Unit.Application.Plans
{
    /// <summary>
    /// Testes unitários do handler de desativação de plano.
    /// </summary>
    public class DeactivatePlanHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPlanRepository> _planRepositoryMock;
        private readonly DeactivatePlanHandler _handler;

        public DeactivatePlanHandlerTests()
        {
            _planRepositoryMock = new Mock<IPlanRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.Plans).Returns(_planRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _handler = new DeactivatePlanHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_WhenPlanIsActive_ShouldDeactivatePlan()
        {
            // Arrange
            var plan = new PlanBuilder()
                .WithIsActive(true)
                .Build();

            _planRepositoryMock
                .Setup(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(plan);

            // Act
            await _handler.Handle(
                new DeactivatePlanCommand(plan.Id),
                CancellationToken.None);

            // Assert
            plan.IsActive.Should().BeFalse();

            _unitOfWorkMock.Verify(
                u => u.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WhenPlanNotFound_ShouldThrowDomainException()
        {
            // Arrange
            _planRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Plan?)null);

            // Act
            var act = async () => await _handler.Handle(
                new DeactivatePlanCommand(Guid.NewGuid()),
                CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*não encontrado*");
        }

        [Fact]
        public async Task Handle_WhenPlanAlreadyInactive_ShouldThrowDomainException()
        {
            // Arrange
            var plan = new PlanBuilder()
                .WithIsActive(false)
                .Build();

            _planRepositoryMock
                .Setup(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(plan);

            // Act
            var act = async () => await _handler.Handle(
                new DeactivatePlanCommand(plan.Id),
                CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*inativo*");
        }
    }
}
