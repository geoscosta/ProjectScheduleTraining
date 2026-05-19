using MediatR;
using ProjectScheduleTraining.Application.Financials.DTOs;

namespace ProjectScheduleTraining.Application.Financials.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para listagem de todas as cobranças de um aluno.
    /// </summary>
    public record GetFinancialsByStudentIdQuery(Guid StudentId) : IRequest<IEnumerable<FinancialSummaryResponse>>;
}
