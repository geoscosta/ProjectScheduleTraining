using ProjectScheduleTraining.Application.Students.DTOs;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Financials.DTOs
{
    /// <summary>
    /// DTO de resposta completa da cobrança financeira.
    /// Utilizado em operações de busca detalhada.
    /// </summary>
    public record FinancialResponse(
        Guid Id,
        Guid StudentId,
        Guid? EnrollmentId,
        decimal Amount,
        DateTime DueDate,
        DateTime? PaymentDate,
        FinancialStatus Status,
        string? Description,
        string? PaymentProof,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        StudentSummaryResponse? Student);

    /// <summary>
    /// DTO de resposta do relatório financeiro mensal.
    /// Consolida os totais de recebimentos, cobranças em aberto e vencidas.
    /// </summary>
    public record FinancialReportResponse(
        int Month,
        int Year,
        decimal TotalReceived,
        decimal TotalPending,
        decimal TotalOverdue,
        int CountReceived,
        int CountPending,
        int CountOverdue);
}
