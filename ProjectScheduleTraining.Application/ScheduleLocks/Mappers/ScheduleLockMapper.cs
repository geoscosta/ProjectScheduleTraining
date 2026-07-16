using ProjectScheduleTraining.Application.ScheduleLocks.DTOs;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Application.ScheduleLocks.Mappers;

/// <summary>
/// Mapper estático para conversão de ScheduleLock → ScheduleLockResponse.
/// Centraliza o mapeamento seguindo o mesmo padrão dos demais mappers do projeto
/// (StudentMapper, EnrollmentMapper, SchedulingMapper etc.).
/// </summary>
public static class ScheduleLockMapper
{
    public static ScheduleLockResponse ToResponse(ScheduleLock scheduleLock)
        => new(
            scheduleLock.Id,
            scheduleLock.StudentId,
            scheduleLock.EnrollmentId,
            scheduleLock.LockStartDate,
            scheduleLock.LockEndDate,
            scheduleLock.Justification,
            scheduleLock.DocumentUrl,
            scheduleLock.Notes,
            scheduleLock.IsApproved,
            scheduleLock.Year,
            scheduleLock.Semester,
            scheduleLock.CreatedAt);
}
