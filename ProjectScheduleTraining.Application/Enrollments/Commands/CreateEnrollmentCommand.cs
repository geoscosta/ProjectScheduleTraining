using MediatR;
using ProjectScheduleTraining.Application.Enrollments.DTOs;

namespace ProjectScheduleTraining.Application.Enrollments.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para criação de uma nova matrícula no sistema.
    /// </summary>
    public record CreateEnrollmentCommand(
        Guid StudentId,
        Guid PlanId,
        int PaymentDueDay) : IRequest<EnrollmentResponse>;
}
