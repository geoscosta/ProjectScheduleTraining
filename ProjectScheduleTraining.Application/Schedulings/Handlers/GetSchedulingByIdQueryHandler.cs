using MediatR;
using ProjectScheduleTraining.Application.Schedulings.DTOs;
using ProjectScheduleTraining.Application.Schedulings.Mappers;
using ProjectScheduleTraining.Application.Schedulings.Queries;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Schedulings.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de busca de um agendamento pelo identificador.
    /// Lança exceção caso o agendamento não seja encontrado.
    /// </summary>
    public class GetSchedulingByIdQueryHandler : IRequestHandler<GetSchedulingByIdQuery, SchedulingResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSchedulingByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de busca do agendamento pelo identificador único.
        /// Lança exceção caso o agendamento não seja encontrado.
        /// </summary>
        public async Task<SchedulingResponse> Handle(
            GetSchedulingByIdQuery request,
            CancellationToken cancellationToken)
        {
            var scheduling = await _unitOfWork.Schedulings
                .GetByIdWithDetailsAsync(request.Id, cancellationToken);

            if (scheduling is null)
                throw new DomainException(
                    "Agendamento não encontrado.",
                    "SCHEDULING_NOT_FOUND");

            return SchedulingMapper.ToResponse(scheduling);
        }
    }
}
