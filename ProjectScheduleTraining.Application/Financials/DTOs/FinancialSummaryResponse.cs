using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Financials.DTOs
{
    /// <summary>
    /// DTO de resposta resumida da cobrança financeira.
    /// Utilizado em listagens e controle de inadimplência.
    /// </summary>
    public record FinancialSummaryResponse(
        Guid Id,
        Guid StudentId,
        decimal Amount,
        DateTime DueDate,
        FinancialStatus Status);
}
