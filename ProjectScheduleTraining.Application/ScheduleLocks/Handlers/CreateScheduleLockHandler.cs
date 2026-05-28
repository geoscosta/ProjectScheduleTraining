using MediatR;
using ProjectScheduleTraining.Application.ScheduleLocks.Commands;
using ProjectScheduleTraining.Application.ScheduleLocks.DTOs;
using ProjectScheduleTraining.CrossCutting.Helpers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.ScheduleLocks.Handlers
{
    /// <summary>
    /// Handler responsável por processar a solicitação de trancamento de agenda.
    /// Valida elegibilidade do aluno e regras de trancamento por semestre.
    /// </summary>
    public class CreateScheduleLockHandler
        : IRequestHandler<CreateScheduleLockCommand, ScheduleLockResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Tipos de plano que dão direito a trancamento de agenda.
        /// Apenas planos fidelidade: Disciplina, Constância e Foco Total.
        /// </summary>
        private static readonly PlanType[] EligiblePlanTypes =
        [
            PlanType.Quarterly,
        PlanType.SemiAnnual,
        PlanType.Annual
        ];

        public CreateScheduleLockHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de trancamento de agenda.
        /// Valida: existência do aluno, elegibilidade do plano,
        /// limite de trancamentos por semestre e período de trancamento.
        /// </summary>
        public async Task<ScheduleLockResponse> Handle(
            CreateScheduleLockCommand request,
            CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.StudentId, cancellationToken);

            if (student is null)
                throw new DomainException(
                    "Aluno não encontrado.",
                    "STUDENT_NOT_FOUND");

            var enrollment = await _unitOfWork.Enrollments
                .GetByIdAsync(request.EnrollmentId, cancellationToken);

            if (enrollment is null || !enrollment.IsActive)
                throw new DomainException(
                    "Matrícula não encontrada ou inativa.",
                    "ENROLLMENT_NOT_FOUND");

            var plan = await _unitOfWork.Plans
                .GetByIdAsync(enrollment.PlanId, cancellationToken);

            /// Valida se o plano do aluno dá direito a trancamento.
            if (plan is null || !EligiblePlanTypes.Contains(plan.Type))
                throw new DomainException(
                    "Apenas alunos com planos Disciplina, Constância ou Foco Total podem trancar a agenda.",
                    "PLAN_NOT_ELIGIBLE_FOR_LOCK");

            /// Determina o semestre e ano do trancamento solicitado.
            var semester = SemesterHelper.GetSemester(request.LockStartDate);
            var year = request.LockStartDate.Year;

            /// Valida se o aluno já utilizou o trancamento no semestre.
            var hasLock = await _unitOfWork.ScheduleLocks
                .HasLockInSemesterAsync(request.StudentId, semester, year, cancellationToken);

            if (hasLock)
                throw new DomainException(
                    $"Você já utilizou o trancamento neste semestre ({SemesterHelper.GetSemesterLabel(semester, year)}). Cada aluno tem direito a 1 trancamento por semestre.",
                    "SCHEDULE_LOCK_ALREADY_USED");

            /// Valida se já existe um trancamento ativo.
            var activeLock = await _unitOfWork.ScheduleLocks
                .GetActiveLockByStudentIdAsync(request.StudentId, cancellationToken);

            if (activeLock is not null)
                throw new DomainException(
                    "Você já possui um trancamento ativo no momento.",
                    "SCHEDULE_LOCK_ALREADY_ACTIVE");

            var scheduleLock = new ScheduleLock
            {
                StudentId = request.StudentId,
                EnrollmentId = request.EnrollmentId,
                LockStartDate = request.LockStartDate,
                LockEndDate = request.LockEndDate,
                Justification = request.Justification,
                DocumentUrl = request.DocumentUrl,
                Notes = request.Notes,
                IsApproved = false,
                Year = year,
                Semester = semester
            };

            await _unitOfWork.ScheduleLocks.AddAsync(scheduleLock, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return MapToResponse(scheduleLock);
        }

        private static ScheduleLockResponse MapToResponse(ScheduleLock scheduleLock)
            => new(
                scheduleLock.Id,
                scheduleLock.StudentId,
                scheduleLock.EnrollmentId,
                scheduleLock.LockStartDate,
                scheduleLock.LockEndDate,
                scheduleLock.Justification,
                scheduleLock.DocumentUrl,
                scheduleLock.Notes,
                scheduleLock.IsApproved,
                scheduleLock.Year,
                scheduleLock.Semester,
                scheduleLock.CreatedAt);
    }
}
