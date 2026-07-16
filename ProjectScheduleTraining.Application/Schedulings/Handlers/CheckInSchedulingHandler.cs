using MediatR;
using ProjectScheduleTraining.Application.Schedulings.Commands;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Schedulings.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de registro de presença.
    /// Valida a existência e o status do agendamento antes de registrar presença.
    /// </summary>
    public class CheckInSchedulingHandler : IRequestHandler<CheckInSchedulingCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckInSchedulingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de registro de presença do aluno.
        /// Lança exceção caso o agendamento não seja encontrado ou já tenha sido processado.
        /// </summary>
        public async Task Handle(
            CheckInSchedulingCommand request,
            CancellationToken cancellationToken)
        {
            var scheduling = await _unitOfWork.Schedulings
                .GetByIdAsync(request.Id, cancellationToken);

            if (scheduling is null)
                throw new DomainException(
                    "Agendamento não encontrado.",
                    "SCHEDULING_NOT_FOUND");

            if (scheduling.Status != SchedulingStatus.Scheduled)
                throw new DomainException(
                    "Não é possível registrar presença para esse agendamento.",
                    "SCHEDULING_INVALID_STATUS");

            scheduling.Status = SchedulingStatus.Present;
            scheduling.TrainerNotes = request.TrainerNotes;

            _unitOfWork.Schedulings.Update(scheduling);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
