using MediatR;
using ProjectScheduleTraining.Application.ScheduleLocks.Commands;
using ProjectScheduleTraining.Application.ScheduleLocks.DTOs;
using ProjectScheduleTraining.Application.ScheduleLocks.Mappers;
using ProjectScheduleTraining.CrossCutting.Helpers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.ScheduleLocks.Handlers;

/// <summary>
/// Handler responsável por processar a solicitação de trancamento de agenda.
/// Valida elegibilidade do aluno e regras de trancamento por semestre.
/// </summary>
public class CreateScheduleLockHandler
    : IRequestHandler<CreateScheduleLockCommand, ScheduleLockResponse>
{
    private readonly IUnitOfWork _unitOfWork;

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

    public async Task<ScheduleLockResponse> Handle(
        CreateScheduleLockCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.Students
            .GetByIdAsync(request.StudentId, cancellationToken);

        if (student is null)
            throw new DomainException("Aluno não encontrado.", "STUDENT_NOT_FOUND");

        var enrollment = await _unitOfWork.Enrollments
            .GetByIdAsync(request.EnrollmentId, cancellationToken);

        if (enrollment is null || !enrollment.IsActive)
            throw new DomainException(
                "Matrícula não encontrada ou inativa.", "ENROLLMENT_NOT_FOUND");

        var plan = await _unitOfWork.Plans
            .GetByIdAsync(enrollment.PlanId, cancellationToken);

        if (plan is null || !EligiblePlanTypes.Contains(plan.Type))
            throw new DomainException(
                "Apenas alunos com planos Disciplina, Constância ou Foco Total podem trancar a agenda.",
                "PLAN_NOT_ELIGIBLE_FOR_LOCK");

        var semester = SemesterHelper.GetSemester(request.LockStartDate);
        var year = request.LockStartDate.Year;

        var hasLock = await _unitOfWork.ScheduleLocks
            .HasLockInSemesterAsync(request.StudentId, semester, year, cancellationToken);

        if (hasLock)
            throw new DomainException(
                $"Você utilizou o trancamento neste semestre ({SemesterHelper.GetSemesterLabel(semester, year)}). Cada aluno tem direito a 1 trancamento por semestre.",
                "SCHEDULE_LOCK_ALREADY_USED");

        var activeLock = await _unitOfWork.ScheduleLocks
            .GetActiveLockByStudentIdAsync(request.StudentId, cancellationToken);

        if (activeLock is not null)
            throw new DomainException(
                "Você possui um trancamento ativo no momento.",
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

        return ScheduleLockMapper.ToResponse(scheduleLock);
    }
}
