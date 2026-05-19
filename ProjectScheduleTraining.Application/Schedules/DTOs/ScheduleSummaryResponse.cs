using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Schedules.DTOs
{
    /// <summary>
    /// DTO de resposta resumida do horário.
    /// Utilizado em listagens e visualização de agenda.
    /// </summary>
    public record ScheduleSummaryResponse(
        Guid Id,
        DateTime Date,
        TimeSpan StartTime,
        TimeSpan EndTime,
        int AvailableSlots,
        ScheduleStatus Status);
}
