using MediatR;

namespace ProjectScheduleTraining.Application.Financials.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para cancelamento de uma cobrança financeira no sistema.
    /// </summary>
    public record CancelFinancialCommand(Guid Id) : IRequest;
}
