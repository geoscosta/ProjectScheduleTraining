using MediatR;
using ProjectScheduleTraining.Application.Plans.DTOs;

namespace ProjectScheduleTraining.Application.Plans.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para listagem de todos os planos ativos do sistema.
    /// </summary>
    public record GetAllPlansQuery : IRequest<IEnumerable<PlanSummaryResponse>>;
}
