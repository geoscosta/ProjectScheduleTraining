using MediatR;
using ProjectScheduleTraining.Application.Financials.DTOs;

namespace ProjectScheduleTraining.Application.Financials.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para criação de uma nova cobrança financeira no sistema.
    /// </summary>
    public record CreateFinancialCommand(
        Guid StudentId,
        Guid? EnrollmentId,
        decimal Amount,
        DateTime DueDate,
        string? Description) : IRequest<FinancialResponse>;
}
