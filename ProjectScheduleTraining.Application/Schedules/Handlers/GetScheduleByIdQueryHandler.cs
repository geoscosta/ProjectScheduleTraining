using MediatR;
using ProjectScheduleTraining.Application.Schedules.DTOs;
using ProjectScheduleTraining.Application.Schedules.Mappers;
using ProjectScheduleTraining.Application.Schedules.Queries;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Schedules.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de busca de um horário pelo identificador.
    /// Lança exceção caso o horário não seja encontrado.
    /// </summary>
    public class GetScheduleByIdQueryHandler : IRequestHandler<GetScheduleByIdQuery, ScheduleResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetScheduleByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de busca do horário pelo identificador único.
        /// Lança exceção caso o horário não seja encontrado.
        /// </summary>
        public async Task<ScheduleResponse> Handle(
            GetScheduleByIdQuery request,
            CancellationToken cancellationToken)
        {
            var schedule = await _unitOfWork.Schedules
                .GetByIdAsync(request.Id, cancellationToken);

            if (schedule is null)
                throw new DomainException(
                    "Horário não encontrado.",
                    "SCHEDULE_NOT_FOUND");

            return ScheduleMapper.ToResponse(schedule);
        }
    }
}
