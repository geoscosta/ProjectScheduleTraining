using ProjectScheduleTraining.Application.Schedules.DTOs;
using ProjectScheduleTraining.Application.Students.DTOs;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Schedulings.DTOs
{
    /// <summary>
    /// DTO de resposta completa do agendamento.
    /// Utilizado em operações de busca detalhada.
    /// </summary>
    public record SchedulingResponse(
        Guid Id,
        Guid StudentId,
        Guid ScheduleId,
        SchedulingStatus Status,
        bool IsMakeup,
        string? JustifiedAbsenceReason,
        string? TrainerNotes,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        StudentSummaryResponse? Student,
        ScheduleSummaryResponse? Schedule);
}
