using MediatR;
using ProjectScheduleTraining.Application.Enrollments.Commands;
using ProjectScheduleTraining.Application.Enrollments.DTOs;
using ProjectScheduleTraining.Application.Enrollments.Mappers;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Enrollments.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de renovação de uma matrícula.
    /// Valida a existência e o status da matrícula antes de renovar.
    /// </summary>
    public class RenewEnrollmentHandler : IRequestHandler<RenewEnrollmentCommand, EnrollmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RenewEnrollmentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de renovação da matrícula.
        /// Lança exceção caso a matrícula não seja encontrada ou esteja cancelada.
        /// </summary>
        public async Task<EnrollmentResponse> Handle(
            RenewEnrollmentCommand request,
            CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.Enrollments
                .GetByIdAsync(request.Id, cancellationToken);

            if (enrollment is null)
                throw new DomainException(
                    "Matrícula não encontrada.",
                    "ENROLLMENT_NOT_FOUND");

            if (!enrollment.IsActive)
                throw new DomainException(
                    "Não é possível renovar uma matrícula cancelada.",
                    "ENROLLMENT_CANCELLED");

            enrollment.ExpirationDate = enrollment.ExpirationDate
                .AddMonths(request.AdditionalMonths);

            _unitOfWork.Enrollments.Update(enrollment);
            await _unitOfWork.CommitAsync(cancellationToken);

            return EnrollmentMapper.ToResponse(enrollment);
        }
    }
}
