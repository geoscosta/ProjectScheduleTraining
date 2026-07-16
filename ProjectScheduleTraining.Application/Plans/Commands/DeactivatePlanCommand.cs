using MediatR;

namespace ProjectScheduleTraining.Application.Plans.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para desativação de um plano no sistema.
    /// </summary>
    public record DeactivatePlanCommand(Guid Id) : IRequest;
}
