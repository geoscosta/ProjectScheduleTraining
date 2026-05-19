using MediatR;
using ProjectScheduleTraining.Application.Schedulings.DTOs;

namespace ProjectScheduleTraining.Application.Schedulings.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para listagem de todos os agendamentos de um aluno.
    /// </summary>
    public record GetSchedulingsByStudentIdQuery(Guid StudentId) : IRequest<IEnumerable<SchedulingSummaryResponse>>;
}
