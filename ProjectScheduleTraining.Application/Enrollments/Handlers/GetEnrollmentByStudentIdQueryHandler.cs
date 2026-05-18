using MediatR;
using ProjectScheduleTraining.Application.Enrollments.DTOs;
using ProjectScheduleTraining.Application.Enrollments.Mappers;
using ProjectScheduleTraining.Application.Enrollments.Queries;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Enrollments.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de busca da matrícula ativa de um aluno.
    /// Lança exceção caso o aluno não possua matrícula ativa.
    /// </summary>
    public class GetEnrollmentByStudentIdQueryHandler : IRequestHandler<GetEnrollmentByStudentIdQuery, EnrollmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEnrollmentByStudentIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de busca da matrícula ativa do aluno.
        /// Lança exceção caso o aluno não possua matrícula ativa.
        /// </summary>
        public async Task<EnrollmentResponse> Handle(
            GetEnrollmentByStudentIdQuery request,
            CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.Enrollments
                .GetByStudentIdAsync(request.StudentId, cancellationToken);

            if (enrollment is null)
                throw new DomainException(
                    "Aluno não possui matrícula ativa.",
                    "ENROLLMENT_NOT_FOUND");

            return EnrollmentMapper.ToResponse(enrollment);
        }
    }
}
