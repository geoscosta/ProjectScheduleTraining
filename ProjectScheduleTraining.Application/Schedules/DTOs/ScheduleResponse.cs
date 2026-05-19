using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Schedules.DTOs
{
    /// <summary>
    /// DTO de resposta completa do horário.
    /// Utilizado em operações de busca detalhada.
    /// </summary>
    public record ScheduleResponse(
        Guid Id,
        DateTime Date,
        TimeSpan StartTime,
        TimeSpan EndTime,
        int MaxCapacity,
        int OccupiedSlots,
        int AvailableSlots,
        ScheduleStatus Status,
        string? Notes,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
