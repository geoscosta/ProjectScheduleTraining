using MediatR;
using ProjectScheduleTraining.Application.Schedules.DTOs;

namespace ProjectScheduleTraining.Application.Schedules.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para listagem de todos os horários de uma data específica.
    /// </summary>
    public record GetSchedulesByDateQuery(DateTime Date) : IRequest<IEnumerable<ScheduleSummaryResponse>>;
}
