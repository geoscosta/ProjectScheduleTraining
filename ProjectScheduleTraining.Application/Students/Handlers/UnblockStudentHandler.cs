using MediatR;
using ProjectScheduleTraining.Application.Students.Commands;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Students.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de desbloqueio de um aluno.
    /// Valida a existência e o status atual do aluno antes de desbloquear.
    /// </summary>
    public class UnblockStudentHandler : IRequestHandler<UnblockStudentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnblockStudentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de desbloqueio do aluno.
        /// Lança exceção caso o aluno não seja encontrado ou não esteja bloqueado.
        /// </summary>
        public async Task Handle(
            UnblockStudentCommand request,
            CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.Id, cancellationToken);

            if (student is null)
                throw new DomainException(
                    "Aluno não encontrado.",
                    "STUDENT_NOT_FOUND");

            if (student.Status != StudentStatus.Blocked)
                throw new DomainException(
                    "Aluno não está bloqueado.",
                    "STUDENT_NOT_BLOCKED");

            student.Status = StudentStatus.Active;

            _unitOfWork.Students.Update(student);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
