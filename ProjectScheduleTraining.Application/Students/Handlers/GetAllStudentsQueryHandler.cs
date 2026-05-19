using MediatR;
using ProjectScheduleTraining.Application.Students.DTOs;
using ProjectScheduleTraining.Application.Students.Mappers;
using ProjectScheduleTraining.Application.Students.Queries;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Students.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de listagem de todos os alunos ativos.
    /// </summary>
    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, IEnumerable<StudentSummaryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllStudentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de listagem de alunos ativos.
        /// Retorna uma lista resumida ordenada por nome.
        /// </summary>
        public async Task<IEnumerable<StudentSummaryResponse>> Handle(
            GetAllStudentsQuery request,
            CancellationToken cancellationToken)
        {
            var students = await _unitOfWork.Students
                .GetActiveStudentsAsync(cancellationToken);

            return students.Select(StudentMapper.ToSummaryResponse);
        }
    }
}
