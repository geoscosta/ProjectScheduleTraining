using MediatR;
using ProjectScheduleTraining.Application.ScheduleLocks.DTOs;

namespace ProjectScheduleTraining.Application.ScheduleLocks.Queries
{
    /// <summary>
    /// Query para listar todos os trancamentos de um aluno.
    /// </summary>
    public record GetScheduleLocksByStudentIdQuery(Guid StudentId)
        : IRequest<IEnumerable<ScheduleLockResponse>>;

    /// <summary>
    /// Query para buscar o trancamento ativo de um aluno.
    /// </summary>
    public record GetActiveScheduleLockQuery(Guid StudentId)
        : IRequest<ScheduleLockResponse?>;
}
