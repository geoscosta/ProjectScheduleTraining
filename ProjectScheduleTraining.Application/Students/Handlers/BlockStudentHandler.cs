using MediatR;
using ProjectScheduleTraining.Application.Students.Commands;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Students.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de bloqueio de um aluno.
    /// Valida a existência e o status atual do aluno antes de bloquear.
    /// </summary>
    public class BlockStudentHandler : IRequestHandler<BlockStudentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlockStudentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de bloqueio do aluno.
        /// Lança exceção caso o aluno não seja encontrado ou já esteja bloqueado.
        /// </summary>
        public async Task Handle(
            BlockStudentCommand request,
            CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.Id, cancellationToken);

            if (student is null)
                throw new DomainException(
                    "Aluno não encontrado.",
                    "STUDENT_NOT_FOUND");

            if (student.Status == StudentStatus.Blocked)
                throw new DomainException(
                    "Aluno já está bloqueado.",
                    "STUDENT_ALREADY_BLOCKED");

            /// Impede o bloqueio se houver matrícula ativa vinculada ao aluno.
            var enrollment = await _unitOfWork.Enrollments
                .GetByStudentIdAsync(request.Id, cancellationToken);

            if (enrollment is not null && enrollment.IsActive)
                throw new DomainException(
                    "Não é possível bloquear um aluno com matrícula ativa. Cancele a matrícula antes de bloquear.",
                    "STUDENT_HAS_ACTIVE_ENROLLMENT");

            student.Status = StudentStatus.Blocked;

            _unitOfWork.Students.Update(student);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
