using MediatR;
using ProjectScheduleTraining.Application.ScheduleLocks.DTOs;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.ScheduleLocks.Commands
{
    /// <summary>
    /// Command para solicitar trancamento de agenda.
    /// Apenas alunos com planos fidelidade têm direito.
    /// </summary>
    public record CreateScheduleLockCommand(
        Guid StudentId,
        Guid EnrollmentId,
        DateTime LockStartDate,
        DateTime LockEndDate,
        LockJustification Justification,
        string? DocumentUrl,
        string? Notes) : IRequest<ScheduleLockResponse>;

    /// <summary>
    /// Command para aprovar um trancamento de agenda.
    /// Apenas administradores podem aprovar.
    /// </summary>
    public record ApproveScheduleLockCommand(Guid Id) : IRequest<ScheduleLockResponse>;

    /// <summary>
    /// Command para rejeitar um trancamento de agenda.
    /// </summary>
    public record RejectScheduleLockCommand(Guid Id, string Reason) : IRequest;
}
