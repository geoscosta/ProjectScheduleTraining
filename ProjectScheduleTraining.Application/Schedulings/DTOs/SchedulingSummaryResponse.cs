using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Schedulings.DTOs
{
    /// <summary>
    /// DTO de resposta resumida do agendamento.
    /// Utilizado em listagens e visualização de agenda.
    /// </summary>
    public record SchedulingSummaryResponse(
        Guid Id,
        Guid StudentId,
        Guid ScheduleId,
        SchedulingStatus Status,
        bool IsMakeup);
}
