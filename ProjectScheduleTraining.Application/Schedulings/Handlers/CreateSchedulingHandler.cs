using MediatR;
using ProjectScheduleTraining.Application.Schedulings.Commands;
using ProjectScheduleTraining.Application.Schedulings.DTOs;
using ProjectScheduleTraining.Application.Schedulings.Mappers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Schedulings.Handlers;

/// <summary>
/// Handler responsável por processar o comando de criação de um novo agendamento.
/// Valida status do aluno, matrícula ativa, disponibilidade do horário e duplicidade.
/// </summary>
public class CreateSchedulingHandler : IRequestHandler<CreateSchedulingCommand, SchedulingResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateSchedulingHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Processa o comando de criação do agendamento.
    /// Ordem de validação:
    /// 1. Aluno existe e está ativo
    /// 2. Aluno possui matrícula ativa
    /// 3. Horário existe e está disponível
    /// 4. Turma não está lotada
    /// 5. Não existe agendamento duplicado
    /// </summary>
    public async Task<SchedulingResponse> Handle(
        CreateSchedulingCommand request,
        CancellationToken cancellationToken)
    {
        /// Valida existência do aluno.
        var student = await _unitOfWork.Students
            .GetByIdAsync(request.StudentId, cancellationToken);

        if (student is null)
            throw new DomainException(
                "Aluno não encontrado.",
                "STUDENT_NOT_FOUND");

        /// Valida se o aluno está ativo para realizar agendamentos.
        if (student.Status == StudentStatus.Inactive)
            throw new DomainException(
                "Não é possível agendar para um aluno inativo.",
                "STUDENT_INACTIVE");

        if (student.Status == StudentStatus.Blocked)
            throw new DomainException(
                "Não é possível agendar para um aluno bloqueado.",
                "STUDENT_BLOCKED");

        /// Valida se o aluno possui matrícula ativa.
        var enrollment = await _unitOfWork.Enrollments
            .GetByStudentIdAsync(request.StudentId, cancellationToken);

        if (enrollment is null || !enrollment.IsActive)
            throw new DomainException(
                "Aluno não possui matrícula ativa. Realize a matrícula antes de agendar.",
                "STUDENT_NO_ACTIVE_ENROLLMENT");

        /// Valida existência e disponibilidade do horário.
        var schedule = await _unitOfWork.Schedules
            .GetByIdAsync(request.ScheduleId, cancellationToken);

        if (schedule is null)
            throw new DomainException(
                "Horário não encontrado.",
                "SCHEDULE_NOT_FOUND");

        if (schedule.Status == ScheduleStatus.Cancelled)
            throw new DomainException(
                "Horário cancelado. Selecione outro horário.",
                "SCHEDULE_CANCELLED");

        if (schedule.Status == ScheduleStatus.Blocked)
            throw new DomainException(
                "Horário bloqueado. Selecione outro horário.",
                "SCHEDULE_BLOCKED");

        if (schedule.Status == ScheduleStatus.Full ||
            schedule.OccupiedSlots >= schedule.MaxCapacity)
            throw new DomainException(
                "Turma lotada. Não há vagas disponíveis nesse horário.",
                "SCHEDULE_FULL");

        /// Valida se já existe agendamento para o aluno neste horário.
        var schedulingExists = await _unitOfWork.Schedulings
            .SchedulingExistsAsync(request.StudentId, request.ScheduleId, cancellationToken);

        if (schedulingExists)
            throw new DomainException(
                "Aluno já possui agendamento nesse horário.",
                "SCHEDULING_ALREADY_EXISTS");

        var scheduling = new Scheduling
        {
            StudentId = request.StudentId,
            ScheduleId = request.ScheduleId,
            Status = SchedulingStatus.Scheduled,
            IsMakeup = request.IsMakeup
        };

        schedule.OccupiedSlots++;

        if (schedule.OccupiedSlots >= schedule.MaxCapacity)
            schedule.Status = ScheduleStatus.Full;

        await _unitOfWork.Schedulings.AddAsync(scheduling, cancellationToken);
        _unitOfWork.Schedules.Update(schedule);
        await _unitOfWork.CommitAsync(cancellationToken);

        scheduling.Student = student;
        scheduling.Schedule = schedule;

        return SchedulingMapper.ToResponse(scheduling);
    }
}