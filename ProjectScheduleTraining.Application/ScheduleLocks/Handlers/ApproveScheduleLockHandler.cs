using MediatR;
using ProjectScheduleTraining.Application.ScheduleLocks.Commands;
using ProjectScheduleTraining.Application.ScheduleLocks.DTOs;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.ScheduleLocks.Handlers
{
    /// <summary>
    /// Handler responsável por aprovar um trancamento de agenda.
    /// Apenas administradores podem aprovar trancamentos.
    /// </summary>
    public class ApproveScheduleLockHandler
        : IRequestHandler<ApproveScheduleLockCommand, ScheduleLockResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApproveScheduleLockHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a aprovação do trancamento de agenda.
        /// </summary>
        public async Task<ScheduleLockResponse> Handle(
            ApproveScheduleLockCommand request,
            CancellationToken cancellationToken)
        {
            var scheduleLock = await _unitOfWork.ScheduleLocks
                .GetByIdAsync(request.Id, cancellationToken);

            if (scheduleLock is null)
                throw new DomainException(
                    "Trancamento não encontrado.",
                    "SCHEDULE_LOCK_NOT_FOUND");

            if (scheduleLock.IsApproved)
                throw new DomainException(
                    "Trancamento já foi aprovado.",
                    "SCHEDULE_LOCK_ALREADY_APPROVED");

            scheduleLock.IsApproved = true;

            _unitOfWork.ScheduleLocks.Update(scheduleLock);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new ScheduleLockResponse(
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
    }
}
