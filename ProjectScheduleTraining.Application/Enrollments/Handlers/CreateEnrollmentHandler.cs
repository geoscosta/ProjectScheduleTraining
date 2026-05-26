using MediatR;
using ProjectScheduleTraining.Application.Enrollments.Commands;
using ProjectScheduleTraining.Application.Enrollments.DTOs;
using ProjectScheduleTraining.Application.Enrollments.Mappers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Enrollments.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de criação de uma nova matrícula.
    /// Valida a existência do aluno e do plano antes de persistir a matrícula.
    /// </summary>
    public class CreateEnrollmentHandler : IRequestHandler<CreateEnrollmentCommand, EnrollmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateEnrollmentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de criação da matrícula.
        /// Valida se o aluno e o plano existem e se o aluno não possui matrícula ativa.
        /// </summary>
        public async Task<EnrollmentResponse> Handle(
            CreateEnrollmentCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Busca o aluno
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.StudentId, cancellationToken);

            if (student is null)
                throw new DomainException("Aluno não encontrado.", "STUDENT_NOT_FOUND");

            // 2. Valida status ANTES de buscar o plano
            if (student.Status == StudentStatus.Inactive)
                throw new DomainException(
                    "Não é possível matricular um aluno inativo.",
                    "STUDENT_INACTIVE");

            if (student.Status == StudentStatus.Blocked)
                throw new DomainException(
                    "Não é possível matricular um aluno bloqueado. Desbloqueie o aluno antes de matricular.",
                    "STUDENT_BLOCKED");

            // 3. Só depois busca o plano
            var plan = await _unitOfWork.Plans
                .GetByIdAsync(request.PlanId, cancellationToken);
            
            if (plan is null)
                throw new DomainException(
                    "Plano não encontrado.",
                    "PLAN_NOT_FOUND");

            if (!plan.IsActive)
                throw new DomainException(
                    "Plano inativo. Selecione um plano ativo.",
                    "PLAN_INACTIVE");

            var activeEnrollment = await _unitOfWork.Enrollments
                .GetByStudentIdAsync(request.StudentId, cancellationToken);

            if (activeEnrollment is not null)
                throw new DomainException(
                    "Aluno já possui uma matrícula ativa.",
                    "ENROLLMENT_ALREADY_EXISTS");

            var enrollment = new Enrollment
            {
                StudentId = request.StudentId,
                PlanId = request.PlanId,
                StartDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMonths(plan.DurationMonths),
                PaymentDueDay = request.PaymentDueDay,
                IsActive = true
            };

            await _unitOfWork.Enrollments.AddAsync(enrollment, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            enrollment.Student = student;
            enrollment.Plan = plan;

            return EnrollmentMapper.ToResponse(enrollment);
        }
    }
}
