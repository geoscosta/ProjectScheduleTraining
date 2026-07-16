using MediatR;
using ProjectScheduleTraining.Application.Schedules.Commands;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Schedules.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de bloqueio de um horário.
    /// Valida a existência e o status atual do horário antes de bloquear.
    /// </summary>
    public class BlockScheduleHandler : IRequestHandler<BlockScheduleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlockScheduleHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de bloqueio do horário.
        /// Lança exceção caso o horário não seja encontrado ou já esteja bloqueado.
        /// </summary>
        public async Task Handle(
            BlockScheduleCommand request,
            CancellationToken cancellationToken)
        {
            var schedule = await _unitOfWork.Schedules
                .GetByIdAsync(request.Id, cancellationToken);

            if (schedule is null)
                throw new DomainException(
                    "Horário não encontrado.",
                    "SCHEDULE_NOT_FOUND");

            if (schedule.Status == ScheduleStatus.Blocked)
                throw new DomainException(
                    "Horário já está bloqueado.",
                    "SCHEDULE_ALREADY_BLOCKED");

            if (schedule.Status == ScheduleStatus.Cancelled)
                throw new DomainException(
                    "Não é possível bloquear um horário cancelado.",
                    "SCHEDULE_CANCELLED");

            schedule.Status = ScheduleStatus.Blocked;
            schedule.Notes = request.Notes;

            _unitOfWork.Schedules.Update(schedule);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
