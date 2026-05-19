using MediatR;
using ProjectScheduleTraining.Application.Schedulings.Commands;
using ProjectScheduleTraining.Application.Schedulings.DTOs;
using ProjectScheduleTraining.Application.Schedulings.Mappers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Schedulings.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de criação de um novo agendamento.
    /// Valida a existência do aluno e do horário, capacidade da turma e duplicidade.
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
        /// Valida se o aluno pode agendar, se há vagas e se não existe agendamento duplicado.
        /// </summary>
        public async Task<SchedulingResponse> Handle(
            CreateSchedulingCommand request,
            CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.StudentId, cancellationToken);

            if (student is null)
                throw new DomainException(
                    "Aluno não encontrado.",
                    "STUDENT_NOT_FOUND");

            if (student.Status != StudentStatus.Active)
                throw new DomainException(
                    "Aluno não está ativo para realizar agendamentos.",
                    "STUDENT_NOT_ACTIVE");

            var schedule = await _unitOfWork.Schedules
                .GetByIdAsync(request.ScheduleId, cancellationToken);

            if (schedule is null)
                throw new DomainException(
                    "Horário não encontrado.",
                    "SCHEDULE_NOT_FOUND");

            if (schedule.Status != ScheduleStatus.Available)
                throw new DomainException(
                    "Horário não está disponível para agendamento.",
                    "SCHEDULE_NOT_AVAILABLE");

            if (schedule.OccupiedSlots >= schedule.MaxCapacity)
                throw new DomainException(
                    "Turma lotada. Não há vagas disponíveis nesse horário.",
                    "SCHEDULE_FULL");

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
}
