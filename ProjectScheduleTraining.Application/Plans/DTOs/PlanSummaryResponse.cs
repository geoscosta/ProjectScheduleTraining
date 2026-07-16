using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Plans.DTOs
{
    /// <summary>
    /// DTO de resposta resumida do plano.
    /// Utilizado em listagens e seleção de planos durante matrícula.
    /// </summary>
    public record PlanSummaryResponse(
        Guid Id,
        string Name,
        PlanType Type,
        WeeklyFrequency WeeklyFrequency,
        int DurationMonths,
        decimal Price);
}
