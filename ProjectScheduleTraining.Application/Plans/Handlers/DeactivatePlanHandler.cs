using MediatR;
using ProjectScheduleTraining.Application.Plans.Commands;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Plans.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de desativação de um plano.
    /// Valida a existência e o status atual do plano antes de desativar.
    /// </summary>
    public class DeactivatePlanHandler : IRequestHandler<DeactivatePlanCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeactivatePlanHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de desativação do plano.
        /// Lança exceção caso o plano não seja encontrado ou já esteja inativo.
        /// </summary>
        public async Task Handle(
            DeactivatePlanCommand request,
            CancellationToken cancellationToken)
        {
            var plan = await _unitOfWork.Plans
                .GetByIdAsync(request.Id, cancellationToken);

            if (plan is null)
                throw new DomainException(
                    "Plano não encontrado.",
                    "PLAN_NOT_FOUND");

            if (!plan.IsActive)
                throw new DomainException(
                    "Plano já está inativo.",
                    "PLAN_ALREADY_INACTIVE");

            plan.IsActive = false;

            _unitOfWork.Plans.Update(plan);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
