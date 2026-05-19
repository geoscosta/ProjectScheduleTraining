using MediatR;
using ProjectScheduleTraining.Application.Schedulings.DTOs;

namespace ProjectScheduleTraining.Application.Schedulings.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para listagem de todos os agendamentos de um horário específico.
    /// Utilizado para controle de presença em uma turma.
    /// </summary>
    public record GetSchedulingsByScheduleIdQuery(Guid ScheduleId) : IRequest<IEnumerable<SchedulingSummaryResponse>>;
}
