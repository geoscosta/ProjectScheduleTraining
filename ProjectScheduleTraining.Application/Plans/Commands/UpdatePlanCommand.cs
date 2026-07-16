using MediatR;
using ProjectScheduleTraining.Application.Plans.DTOs;

namespace ProjectScheduleTraining.Application.Plans.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para atualização de um plano existente no sistema.
    /// </summary>
    public record UpdatePlanCommand(
        Guid Id,
        string Name,
        decimal Price,
        string? Description) : IRequest<PlanResponse>;
}
