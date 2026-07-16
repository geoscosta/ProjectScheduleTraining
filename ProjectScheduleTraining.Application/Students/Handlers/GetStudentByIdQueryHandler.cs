using MediatR;
using ProjectScheduleTraining.Application.Students.DTOs;
using ProjectScheduleTraining.Application.Students.Mappers;
using ProjectScheduleTraining.Application.Students.Queries;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Students.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de busca de um aluno pelo identificador.
    /// Lança exceção caso o aluno não seja encontrado.
    /// </summary>
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStudentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de busca do aluno pelo identificador único.
        /// Lança exceção caso o aluno não seja encontrado.
        /// </summary>
        public async Task<StudentResponse> Handle(
            GetStudentByIdQuery request,
            CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.Id, cancellationToken);

            if (student is null)
                throw new DomainException(
                    "Aluno não encontrado.",
                    "STUDENT_NOT_FOUND");

            return StudentMapper.ToResponse(student);
        }
    }
}
