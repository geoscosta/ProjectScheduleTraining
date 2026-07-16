namespace ProjectScheduleTraining.Application.Enrollments.DTOs
{
    /// <summary>
    /// DTO de resposta resumida da matrícula.
    /// Utilizado em listagens e buscas gerais.
    /// </summary>
    public record EnrollmentSummaryResponse(
        Guid Id,
        Guid StudentId,
        Guid PlanId,
        DateTime StartDate,
        DateTime ExpirationDate,
        bool IsActive);
}
