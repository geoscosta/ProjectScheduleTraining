using MediatR;
using ProjectScheduleTraining.Application.ScheduleLocks.DTOs;
using ProjectScheduleTraining.Application.ScheduleLocks.Queries;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.ScheduleLocks.Handlers
{
    /// <summary>
    /// Handler responsável por buscar trancamentos de agenda.
    /// </summary>
    public class GetScheduleLockHandler
        : IRequestHandler<GetScheduleLocksByStudentIdQuery, IEnumerable<ScheduleLockResponse>>,
          IRequestHandler<GetActiveScheduleLockQuery, ScheduleLockResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetScheduleLockHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retorna todos os trancamentos de um aluno.
        /// </summary>
        public async Task<IEnumerable<ScheduleLockResponse>> Handle(
            GetScheduleLocksByStudentIdQuery request,
            CancellationToken cancellationToken)
        {
            var locks = await _unitOfWork.ScheduleLocks
                .GetByStudentIdAsync(request.StudentId, cancellationToken);

            return locks.Select(MapToResponse);
        }

        /// <summary>
        /// Retorna o trancamento ativo de um aluno se existir.
        /// </summary>
        public async Task<ScheduleLockResponse?> Handle(
            GetActiveScheduleLockQuery request,
            CancellationToken cancellationToken)
        {
            var scheduleLock = await _unitOfWork.ScheduleLocks
                .GetActiveLockByStudentIdAsync(request.StudentId, cancellationToken);

            return scheduleLock is null ? null : MapToResponse(scheduleLock);
        }

        private static ScheduleLockResponse MapToResponse(ScheduleLock scheduleLock)
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
}
