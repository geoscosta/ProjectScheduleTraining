using MediatR;
using ProjectScheduleTraining.Application.Schedules.Commands;
using ProjectScheduleTraining.Application.Schedules.DTOs;
using ProjectScheduleTraining.Application.Schedules.Mappers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Schedules.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de criação de um novo horário na agenda.
    /// Valida conflito de horários antes de persistir.
    /// </summary>
    public class CreateScheduleHandler : IRequestHandler<CreateScheduleCommand, ScheduleResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateScheduleHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de criação do horário.
        /// Verifica se já existe um horário cadastrado na mesma data e hora.
        /// </summary>
        public async Task<ScheduleResponse> Handle(
            CreateScheduleCommand request,
            CancellationToken cancellationToken)
        {
            var schedulesOnDate = await _unitOfWork.Schedules
                .GetByDateAsync(request.Date, cancellationToken);

            var timeConflict = schedulesOnDate.Any(s =>
                s.StartTime == request.StartTime &&
                s.Status != ScheduleStatus.Cancelled);

            if (timeConflict)
                throw new DomainException(
                    "Já existe um horário cadastrado nessa data e hora.",
                    "SCHEDULE_TIME_CONFLICT");

            var schedule = new Schedule
            {
                Date = request.Date.Date,
                StartTime = request.StartTime,
                EndTime = request.StartTime.Add(TimeSpan.FromHours(1)),
                MaxCapacity = 5,
                OccupiedSlots = 0,
                Status = ScheduleStatus.Available
            };

            await _unitOfWork.Schedules.AddAsync(schedule, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return ScheduleMapper.ToResponse(schedule);
        }
    }
}
