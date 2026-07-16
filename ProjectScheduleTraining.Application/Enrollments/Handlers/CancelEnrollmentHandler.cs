using MediatR;
using ProjectScheduleTraining.Application.Enrollments.Commands;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Enrollments.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de cancelamento de uma matrícula.
    /// Valida a existência e o status da matrícula antes de cancelar.
    /// </summary>
    public class CancelEnrollmentHandler : IRequestHandler<CancelEnrollmentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelEnrollmentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de cancelamento da matrícula.
        /// Lança exceção caso a matrícula não seja encontrada ou já esteja cancelada.
        /// </summary>
        public async Task Handle(
            CancelEnrollmentCommand request,
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
                    "Matrícula já está cancelada.",
                    "ENROLLMENT_ALREADY_CANCELLED");

            enrollment.IsActive = false;

            _unitOfWork.Enrollments.Update(enrollment);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
