using MediatR;
using ProjectScheduleTraining.Application.Financials.DTOs;

namespace ProjectScheduleTraining.Application.Financials.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para listagem de todas as cobranças vencidas do sistema.
    /// Utilizado para controle de inadimplência.
    /// </summary>
    public record GetOverdueFinancialsQuery : IRequest<IEnumerable<FinancialSummaryResponse>>;
}
