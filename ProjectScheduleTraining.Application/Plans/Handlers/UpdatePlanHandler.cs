using MediatR;
using ProjectScheduleTraining.Application.Plans.Commands;
using ProjectScheduleTraining.Application.Plans.DTOs;
using ProjectScheduleTraining.Application.Plans.Mappers;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Plans.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de atualização de um plano existente.
    /// Valida a existência do plano antes de persistir as alterações.
    /// </summary>
    public class UpdatePlanHandler : IRequestHandler<UpdatePlanCommand, PlanResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePlanHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de atualização do plano.
        /// Lança exceção caso o plano não seja encontrado.
        /// </summary>
        public async Task<PlanResponse> Handle(
            UpdatePlanCommand request,
            CancellationToken cancellationToken)
        {
            var plan = await _unitOfWork.Plans
                .GetByIdAsync(request.Id, cancellationToken);

            if (plan is null)
                throw new DomainException(
                    "Plano não encontrado.",
                    "PLAN_NOT_FOUND");

            plan.Name = request.Name.Trim();
            plan.Price = request.Price;
            plan.Description = request.Description?.Trim();

            _unitOfWork.Plans.Update(plan);
            await _unitOfWork.CommitAsync(cancellationToken);

            return PlanMapper.ToResponse(plan);
        }
    }
}
