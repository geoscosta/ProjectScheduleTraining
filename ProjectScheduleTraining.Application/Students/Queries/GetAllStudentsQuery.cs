using MediatR;
using ProjectScheduleTraining.Application.Students.DTOs;

namespace ProjectScheduleTraining.Application.Students.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para listagem de todos os alunos ativos do sistema.
    /// </summary>
    public record GetAllStudentsQuery : IRequest<IEnumerable<StudentSummaryResponse>>;
}
