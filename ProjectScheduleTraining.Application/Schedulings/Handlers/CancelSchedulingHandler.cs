using MediatR;
using ProjectScheduleTraining.Application.Schedulings.Commands;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Schedulings.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de cancelamento de um agendamento.
    /// Valida a existência e o status do agendamento antes de cancelar.
    /// Libera a vaga no horário ao cancelar o agendamento.
    /// </summary>
    public class CancelSchedulingHandler : IRequestHandler<CancelSchedulingCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelSchedulingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de cancelamento do agendamento.
        /// Libera a vaga no horário e atualiza o status do horário se necessário.
        /// </summary>
        public async Task Handle(
            CancelSchedulingCommand request,
            CancellationToken cancellationToken)
        {
            var scheduling = await _unitOfWork.Schedulings
                .GetByIdAsync(request.Id, cancellationToken);

            if (scheduling is null)
                throw new DomainException(
                    "Agendamento não encontrado.",
                    "SCHEDULING_NOT_FOUND");

            if (scheduling.Status == SchedulingStatus.Cancelled)
                throw new DomainException(
                    "Agendamento já está cancelado.",
                    "SCHEDULING_ALREADY_CANCELLED");

            if (scheduling.Status == SchedulingStatus.Present)
                throw new DomainException(
                    "Não é possível cancelar uma aula já realizada.",
                    "SCHEDULING_ALREADY_PRESENT");

            var schedule = await _unitOfWork.Schedules
                .GetByIdAsync(scheduling.ScheduleId, cancellationToken);

            if (schedule is not null)
            {
                schedule.OccupiedSlots--;

                if (schedule.Status == ScheduleStatus.Full)
                    schedule.Status = ScheduleStatus.Available;

                _unitOfWork.Schedules.Update(schedule);
            }

            scheduling.Status = SchedulingStatus.Cancelled;

            _unitOfWork.Schedulings.Update(scheduling);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
