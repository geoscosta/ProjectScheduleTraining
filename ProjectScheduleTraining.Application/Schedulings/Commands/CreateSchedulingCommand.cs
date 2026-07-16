using MediatR;
using ProjectScheduleTraining.Application.Schedulings.DTOs;

namespace ProjectScheduleTraining.Application.Schedulings.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para criação de um novo agendamento no sistema.
    /// </summary>
    public record CreateSchedulingCommand(
        Guid StudentId,
        Guid ScheduleId,
        bool IsMakeup) : IRequest<SchedulingResponse>;
}
