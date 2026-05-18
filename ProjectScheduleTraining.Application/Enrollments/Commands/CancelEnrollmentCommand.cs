using MediatR;

namespace ProjectScheduleTraining.Application.Enrollments.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para cancelamento de uma matrícula no sistema.
    /// </summary>
    public record CancelEnrollmentCommand(Guid Id) : IRequest;
}
