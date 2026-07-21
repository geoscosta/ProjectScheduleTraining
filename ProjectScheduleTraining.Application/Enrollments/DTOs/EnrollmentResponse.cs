using ProjectScheduleTraining.Application.Plans.DTOs;
using ProjectScheduleTraining.Application.Students.DTOs;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Enrollments.DTOs
{
    /// <summary>
    /// DTO de resposta completa da matrícula.
    /// Utilizado em operações de busca detalhada.
    /// </summary>
    public record EnrollmentResponse(
        Guid Id,
        Guid StudentId,
        Guid PlanId,
        DateTime StartDate,
        DateTime ExpirationDate,
        PaymentDueDay PaymentDueDay,
        PaymentMethod PaymentMethod,
        decimal DiscountPercentage,
        decimal FinalPrice,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        StudentSummaryResponse? Student,
        PlanSummaryResponse? Plan);
}
