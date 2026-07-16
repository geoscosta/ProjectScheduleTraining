using MediatR;
using ProjectScheduleTraining.Application.Financials.DTOs;

namespace ProjectScheduleTraining.Application.Financials.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para busca de uma cobrança pelo seu identificador único.
    /// </summary>
    public record GetFinancialByIdQuery(Guid Id) : IRequest<FinancialResponse>;
}
