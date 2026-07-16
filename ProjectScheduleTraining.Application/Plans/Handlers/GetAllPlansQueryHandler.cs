using MediatR;
using ProjectScheduleTraining.Application.Plans.DTOs;
using ProjectScheduleTraining.Application.Plans.Mappers;
using ProjectScheduleTraining.Application.Plans.Queries;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Plans.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de listagem de todos os planos ativos.
    /// </summary>
    public class GetAllPlansQueryHandler : IRequestHandler<GetAllPlansQuery, IEnumerable<PlanSummaryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPlansQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de listagem de planos ativos.
        /// Retorna uma lista resumida ordenada por nome.
        /// </summary>
        public async Task<IEnumerable<PlanSummaryResponse>> Handle(
            GetAllPlansQuery request,
            CancellationToken cancellationToken)
        {
            var plans = await _unitOfWork.Plans
                .GetActivePlansAsync(cancellationToken);

            return plans.Select(PlanMapper.ToSummaryResponse);
        }
    }
}
