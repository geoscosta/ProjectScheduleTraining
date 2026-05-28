using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.ScheduleLocks.DTOs
{
    /// <summary>
    /// DTO de resposta de trancamento de agenda.
    /// </summary>
    public record ScheduleLockResponse(
        Guid Id,
        Guid StudentId,
        Guid EnrollmentId,
        DateTime LockStartDate,
        DateTime LockEndDate,
        LockJustification Justification,
        string? DocumentUrl,
        string? Notes,
        bool IsApproved,
        int Year,
        Semester Semester,
        DateTime CreatedAt);
}
