using MediatR;
using ProjectScheduleTraining.Application.Schedulings.DTOs;

namespace ProjectScheduleTraining.Application.Schedulings.Commands;

/// <summary>
/// Command responsável por transportar os dados necessários
/// para criação de um novo agendamento.
/// </summary>
public record CreateSchedulingCommand(
    Guid StudentId,
    Guid ScheduleId,
    bool IsMakeup,
    bool HasMedicalCertificate = false)
    : IRequest<SchedulingResponse>;