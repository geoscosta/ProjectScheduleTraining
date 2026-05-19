using MediatR;
using ProjectScheduleTraining.Application.Financials.DTOs;

namespace ProjectScheduleTraining.Application.Financials.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para registro de pagamento de uma cobrança financeira.
    /// </summary>
    public record RegisterPaymentCommand(
        Guid Id,
        string? PaymentProof) : IRequest<FinancialResponse>;
}
