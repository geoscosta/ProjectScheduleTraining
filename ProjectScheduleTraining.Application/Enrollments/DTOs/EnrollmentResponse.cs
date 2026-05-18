using ProjectScheduleTraining.Application.Plans.DTOs;
using ProjectScheduleTraining.Application.Students.DTOs;

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
        int PaymentDueDay,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        StudentSummaryResponse? Student,
        PlanSummaryResponse? Plan);
}
