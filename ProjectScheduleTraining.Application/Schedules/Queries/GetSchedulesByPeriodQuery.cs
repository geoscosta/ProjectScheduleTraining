using MediatR;
using ProjectScheduleTraining.Application.Schedules.DTOs;

namespace ProjectScheduleTraining.Application.Schedules.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para listagem de todos os horários de um período.
    /// Utilizado para visualização semanal e mensal da agenda.
    /// </summary>
    public record GetSchedulesByPeriodQuery(
        DateTime Start,
        DateTime End) : IRequest<IEnumerable<ScheduleSummaryResponse>>;
}
