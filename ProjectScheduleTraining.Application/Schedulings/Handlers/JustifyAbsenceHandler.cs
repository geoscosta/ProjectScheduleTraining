using MediatR;
using ProjectScheduleTraining.Application.Schedulings.Commands;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Schedulings.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de registro de falta justificada.
    /// Valida a existência e o status do agendamento antes de registrar a justificativa.
    /// </summary>
    public class JustifyAbsenceHandler : IRequestHandler<JustifyAbsenceCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public JustifyAbsenceHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de registro de falta justificada.
        /// Lança exceção caso o agendamento não seja encontrado ou já tenha sido processado.
        /// </summary>
        public async Task Handle(
            JustifyAbsenceCommand request,
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
                    "Não é possível justificar falta para esse agendamento.",
                    "SCHEDULING_INVALID_STATUS");

            scheduling.Status = SchedulingStatus.JustifiedAbsence;
            scheduling.JustifiedAbsenceReason = request.Reason;

            _unitOfWork.Schedulings.Update(scheduling);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
