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
/// Executa dentro de uma transação explícita para evitar race condition em OccupiedSlots:
/// sem transação, dois requests simultâneos poderiam passar pela verificação de capacidade
/// e ambos inserir agendamentos em uma turma com apenas 1 vaga restante.
/// </summary>
public class CreateSchedulingHandler : IRequestHandler<CreateSchedulingCommand, SchedulingResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateSchedulingHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Processa o comando de criação do agendamento dentro de uma transação.
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
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.StudentId, cancellationToken);

            if (student is null)
                throw new DomainException("Aluno não encontrado.", "STUDENT_NOT_FOUND");

            if (student.Status == StudentStatus.Inactive)
                throw new DomainException(
                    "Não é possível agendar para um aluno inativo.", "STUDENT_INACTIVE");

            if (student.Status == StudentStatus.Blocked)
                throw new DomainException(
                    "Não é possível agendar para um aluno bloqueado.", "STUDENT_BLOCKED");

            var enrollment = await _unitOfWork.Enrollments
                .GetByStudentIdAsync(request.StudentId, cancellationToken);

            if (enrollment is null || !enrollment.IsActive)
                throw new DomainException(
                    "Aluno não possui matrícula ativa. Realize a matrícula antes de agendar.",
                    "STUDENT_NO_ACTIVE_ENROLLMENT");

            var schedule = await _unitOfWork.Schedules
                .GetByIdAsync(request.ScheduleId, cancellationToken);

            if (schedule is null)
                throw new DomainException("Horário não encontrado.", "SCHEDULE_NOT_FOUND");

            if (schedule.Status == ScheduleStatus.Cancelled)
                throw new DomainException(
                    "Horário cancelado. Selecione outro horário.", "SCHEDULE_CANCELLED");

            if (schedule.Status == ScheduleStatus.Blocked)
                throw new DomainException(
                    "Horário bloqueado. Selecione outro horário.", "SCHEDULE_BLOCKED");

            // A verificação de capacidade e o incremento estão dentro da transação.
            // Isso garante que dois requests concorrentes não passem ambos por aqui
            // com OccupiedSlots = MaxCapacity - 1 e ambos insiram um agendamento a mais.
            if (schedule.Status == ScheduleStatus.Full ||
                schedule.OccupiedSlots >= schedule.MaxCapacity)
                throw new DomainException(
                    "Turma lotada. Não há vagas disponíveis nesse horário.", "SCHEDULE_FULL");

            var schedulingExists = await _unitOfWork.Schedulings
                .SchedulingExistsAsync(request.StudentId, request.ScheduleId, cancellationToken);

            if (schedulingExists)
                throw new DomainException(
                    "Aluno já possui agendamento nesse horário.", "SCHEDULING_ALREADY_EXISTS");

            /// Valida regras de reposição quando IsMakeup = true.
            if (request.IsMakeup)
            {
                /// Atestado médico obrigatório para TODAS as reposições.
                if (!request.HasMedicalCertificate)
                    throw new DomainException(
                        "Reposições somente são permitidas mediante atestado médico.",
                        "MAKEUP_MEDICAL_CERTIFICATE_REQUIRED");

                /// Verifica antecedência mínima de 12 horas para reposição.
                var scheduleDateTime = schedule.Date.Add(schedule.StartTime);
                var hoursUntilClass = (scheduleDateTime - DateTime.UtcNow).TotalHours;

                if (hoursUntilClass < 12)
                    throw new DomainException(
                        "Reposições devem ser agendadas com no mínimo 12 horas de antecedência.",
                        "MAKEUP_INSUFFICIENT_NOTICE");

                /// Limite de 2 reposições por mês — agora atestado é obrigatório
                /// mas ainda conta no limite (não há mais isenção por atestado).
                var makeupCount = await _unitOfWork.Schedulings
                        .CountMakeupSchedulingsInLast30DaysAsync(
                            request.StudentId,
                            cancellationToken);

                if (makeupCount >= 2)
                    throw new DomainException(
                        "Limite de 2 reposições por mês atingido.",
                        "MAKEUP_LIMIT_EXCEEDED");
            }

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
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            scheduling.Student = student;
            scheduling.Schedule = schedule;

            return SchedulingMapper.ToResponse(scheduling);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
