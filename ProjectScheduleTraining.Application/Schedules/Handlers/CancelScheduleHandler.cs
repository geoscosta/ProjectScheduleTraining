using MediatR;
using ProjectScheduleTraining.Application.Schedules.Commands;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Schedules.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de cancelamento de um horário.
    /// Valida a existência e o status atual do horário antes de cancelar.
    /// </summary>
    public class CancelScheduleHandler : IRequestHandler<CancelScheduleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelScheduleHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de cancelamento do horário.
        /// Lança exceção caso o horário não seja encontrado ou já esteja cancelado.
        /// </summary>
        public async Task Handle(
            CancelScheduleCommand request,
            CancellationToken cancellationToken)
        {
            var schedule = await _unitOfWork.Schedules
                .GetByIdAsync(request.Id, cancellationToken);

            if (schedule is null)
                throw new DomainException(
                    "Horário não encontrado.",
                    "SCHEDULE_NOT_FOUND");

            if (schedule.Status == ScheduleStatus.Cancelled)
                throw new DomainException(
                    "Horário já está cancelado.",
                    "SCHEDULE_ALREADY_CANCELLED");

            schedule.Status = ScheduleStatus.Cancelled;

            _unitOfWork.Schedules.Update(schedule);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
