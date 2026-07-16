using MediatR;
using ProjectScheduleTraining.Application.Schedulings.DTOs;
using ProjectScheduleTraining.Application.Schedulings.Mappers;
using ProjectScheduleTraining.Application.Schedulings.Queries;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Schedulings.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de listagem de agendamentos de um horário.
    /// Utilizado para controle de presença em uma turma.
    /// </summary>
    public class GetSchedulingsByScheduleIdQueryHandler : IRequestHandler<GetSchedulingsByScheduleIdQuery, IEnumerable<SchedulingSummaryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSchedulingsByScheduleIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de listagem de agendamentos de um horário específico.
        /// Retorna todos os alunos agendados para aquele horário.
        /// </summary>
        public async Task<IEnumerable<SchedulingSummaryResponse>> Handle(
            GetSchedulingsByScheduleIdQuery request,
            CancellationToken cancellationToken)
        {
            var schedulings = await _unitOfWork.Schedulings
                .GetByScheduleIdAsync(request.ScheduleId, cancellationToken);

            return schedulings.Select(SchedulingMapper.ToSummaryResponse);
        }
    }
}
