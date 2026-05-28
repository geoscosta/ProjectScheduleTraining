using MediatR;
using ProjectScheduleTraining.Application.ScheduleLocks.Commands;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.ScheduleLocks.Handlers
{
    /// <summary>
    /// Handler responsável por rejeitar um trancamento de agenda.
    /// Apenas administradores podem rejeitar trancamentos.
    /// </summary>
    public class RejectScheduleLockHandler
        : IRequestHandler<RejectScheduleLockCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RejectScheduleLockHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a rejeição do trancamento de agenda.
        /// Realiza soft delete do trancamento rejeitado.
        /// </summary>
        public async Task Handle(
            RejectScheduleLockCommand request,
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
                    "Não é possível rejeitar um trancamento já aprovado.",
                    "SCHEDULE_LOCK_ALREADY_APPROVED");

            /// Registra o motivo da rejeição nas observações antes de deletar.
            scheduleLock.Notes = $"REJEITADO: {request.Reason}. {scheduleLock.Notes}";

            _unitOfWork.ScheduleLocks.Delete(scheduleLock);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
