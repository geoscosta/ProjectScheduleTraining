using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Plans.DTOs
{
    /// <summary>
    /// DTO de resposta completa do plano.
    /// Utilizado em operações de busca detalhada.
    /// </summary>
    public record PlanResponse(
        Guid Id,
        string Name,
        PlanType Type,
        WeeklyFrequency WeeklyFrequency,
        int DurationMonths,
        decimal Price,
        bool IsActive,
        string? Description,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
