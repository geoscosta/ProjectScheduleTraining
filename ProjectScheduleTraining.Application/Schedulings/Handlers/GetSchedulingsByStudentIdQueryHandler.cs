using MediatR;
using ProjectScheduleTraining.Application.Schedulings.DTOs;
using ProjectScheduleTraining.Application.Schedulings.Mappers;
using ProjectScheduleTraining.Application.Schedulings.Queries;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Schedulings.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de listagem de agendamentos de um aluno.
    /// </summary>
    public class GetSchedulingsByStudentIdQueryHandler : IRequestHandler<GetSchedulingsByStudentIdQuery, IEnumerable<SchedulingSummaryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSchedulingsByStudentIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de listagem de agendamentos do aluno.
        /// Retorna todos os agendamentos ordenados do mais recente para o mais antigo.
        /// </summary>
        public async Task<IEnumerable<SchedulingSummaryResponse>> Handle(
            GetSchedulingsByStudentIdQuery request,
            CancellationToken cancellationToken)
        {
            var schedulings = await _unitOfWork.Schedulings
                .GetByStudentIdAsync(request.StudentId, cancellationToken);

            return schedulings.Select(SchedulingMapper.ToSummaryResponse);
        }
    }
}
