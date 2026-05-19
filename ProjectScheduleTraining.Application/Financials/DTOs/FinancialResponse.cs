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
}
