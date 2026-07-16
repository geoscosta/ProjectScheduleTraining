using MediatR;
using ProjectScheduleTraining.Application.Plans.DTOs;
using ProjectScheduleTraining.Application.Plans.Mappers;
using ProjectScheduleTraining.Application.Plans.Queries;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Plans.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de busca de um plano pelo identificador.
    /// Lança exceção caso o plano não seja encontrado.
    /// </summary>
    public class GetPlanByIdQueryHandler : IRequestHandler<GetPlanByIdQuery, PlanResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPlanByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de busca do plano pelo identificador único.
        /// Lança exceção caso o plano não seja encontrado.
        /// </summary>
        public async Task<PlanResponse> Handle(
            GetPlanByIdQuery request,
            CancellationToken cancellationToken)
        {
            var plan = await _unitOfWork.Plans
                .GetByIdAsync(request.Id, cancellationToken);

            if (plan is null)
                throw new DomainException(
                    "Plano não encontrado.",
                    "PLAN_NOT_FOUND");

            return PlanMapper.ToResponse(plan);
        }
    }
}
