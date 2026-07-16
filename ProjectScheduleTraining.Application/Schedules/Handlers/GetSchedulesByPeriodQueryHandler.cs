using MediatR;
using ProjectScheduleTraining.Application.Schedules.DTOs;
using ProjectScheduleTraining.Application.Schedules.Mappers;
using ProjectScheduleTraining.Application.Schedules.Queries;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Schedules.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de listagem de horários por período.
    /// </summary>
    public class GetSchedulesByPeriodQueryHandler : IRequestHandler<GetSchedulesByPeriodQuery, IEnumerable<ScheduleSummaryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSchedulesByPeriodQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de listagem de horários de um período.
        /// Valida se a data de início é anterior à data de fim.
        /// </summary>
        public async Task<IEnumerable<ScheduleSummaryResponse>> Handle(
            GetSchedulesByPeriodQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Start > request.End)
                throw new DomainException(
                    "Data de início não pode ser posterior à data de fim.",
                    "SCHEDULE_INVALID_PERIOD");

            var schedules = await _unitOfWork.Schedules
                .GetByPeriodAsync(request.Start, request.End, cancellationToken);

            return schedules.Select(ScheduleMapper.ToSummaryResponse);
        }
    }
}
