using MediatR;
using ProjectScheduleTraining.Application.Schedules.DTOs;
using ProjectScheduleTraining.Application.Schedules.Mappers;
using ProjectScheduleTraining.Application.Schedules.Queries;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Schedules.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de listagem de horários por data.
    /// </summary>
    public class GetSchedulesByDateQueryHandler : IRequestHandler<GetSchedulesByDateQuery, IEnumerable<ScheduleSummaryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSchedulesByDateQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de listagem de horários de uma data específica.
        /// Retorna todos os horários ordenados por hora de início.
        /// </summary>
        public async Task<IEnumerable<ScheduleSummaryResponse>> Handle(
            GetSchedulesByDateQuery request,
            CancellationToken cancellationToken)
        {
            var schedules = await _unitOfWork.Schedules
                .GetByDateAsync(request.Date, cancellationToken);

            return schedules.Select(ScheduleMapper.ToSummaryResponse);
        }
    }
}
