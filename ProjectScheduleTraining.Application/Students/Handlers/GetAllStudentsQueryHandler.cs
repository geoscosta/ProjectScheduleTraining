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
        /// Processa a query de listagem de alunos.
        /// Utiliza GetAllAsync para retornar todos os alunos não deletados,
        /// incluindo os bloqueados, pois o soft delete já filtra os inativados.
        /// </summary>
        public async Task<IEnumerable<StudentSummaryResponse>> Handle(
            GetAllStudentsQuery request,
            CancellationToken cancellationToken)
        {
            var students = await _unitOfWork.Students
                .GetAllAsync(cancellationToken);

            return students
                .OrderBy(s => s.Name)
                .Select(StudentMapper.ToSummaryResponse);
        }
    }
}
