using MediatR;
using ProjectScheduleTraining.Application.Schedulings.DTOs;

namespace ProjectScheduleTraining.Application.Schedulings.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para busca de um agendamento pelo seu identificador único.
    /// </summary>
    public record GetSchedulingByIdQuery(Guid Id) : IRequest<SchedulingResponse>;
}
