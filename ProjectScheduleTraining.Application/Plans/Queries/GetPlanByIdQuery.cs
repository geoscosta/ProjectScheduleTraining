using MediatR;
using ProjectScheduleTraining.Application.Plans.DTOs;

namespace ProjectScheduleTraining.Application.Plans.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para busca de um plano pelo seu identificador único.
    /// </summary>
    public record GetPlanByIdQuery(Guid Id) : IRequest<PlanResponse>;
}
