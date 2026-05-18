using MediatR;
using ProjectScheduleTraining.Application.Enrollments.DTOs;
using ProjectScheduleTraining.Application.Enrollments.Mappers;
using ProjectScheduleTraining.Application.Enrollments.Queries;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Enrollments.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de busca de uma matrícula pelo identificador.
    /// Lança exceção caso a matrícula não seja encontrada.
    /// </summary>
    public class GetEnrollmentByIdQueryHandler : IRequestHandler<GetEnrollmentByIdQuery, EnrollmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEnrollmentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de busca da matrícula pelo identificador único.
        /// Lança exceção caso a matrícula não seja encontrada.
        /// </summary>
        public async Task<EnrollmentResponse> Handle(
            GetEnrollmentByIdQuery request,
            CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.Enrollments
                .GetByIdAsync(request.Id, cancellationToken);

            if (enrollment is null)
                throw new DomainException(
                    "Matrícula não encontrada.",
                    "ENROLLMENT_NOT_FOUND");

            return EnrollmentMapper.ToResponse(enrollment);
        }
    }
}
