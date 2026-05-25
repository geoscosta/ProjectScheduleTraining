using MediatR;
using ProjectScheduleTraining.Application.Students.Commands;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Students.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de inativação de um aluno.
    /// Valida a existência do aluno antes de inativar.
    /// </summary>
    public class DeactivateStudentHandler : IRequestHandler<DeactivateStudentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateStudentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de inativação do aluno.
        /// Lança exceção caso o aluno não seja encontrado ou já esteja inativo.
        /// </summary>
        public async Task Handle(
            DeactivateStudentCommand request,
            CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.Id, cancellationToken);

            if (student is null)
                throw new DomainException(
                    "Aluno não encontrado.",
                    "STUDENT_NOT_FOUND");

            if (student.Status == StudentStatus.Inactive)
                throw new DomainException(
                    "Aluno já está inativo.",
                    "STUDENT_ALREADY_INACTIVE");

            /// Impede a inativação se houver matrícula ativa vinculada ao aluno.
            /// O usuário deve cancelar a matrícula antes de inativar o aluno.
            var enrollment = await _unitOfWork.Enrollments
                .GetByStudentIdAsync(request.Id, cancellationToken);

            if (enrollment is not null && enrollment.IsActive)
                throw new DomainException(
                    "Não é possível inativar um aluno com matrícula ativa. Cancele a matrícula antes de inativar.",
                    "STUDENT_HAS_ACTIVE_ENROLLMENT");

            student.Status = StudentStatus.Inactive;

            _unitOfWork.Students.Update(student);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
