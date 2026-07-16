using MediatR;
using ProjectScheduleTraining.Application.Enrollments.DTOs;

namespace ProjectScheduleTraining.Application.Enrollments.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para renovação de uma matrícula existente no sistema.
    /// </summary>
    public record RenewEnrollmentCommand(
        Guid Id,
        int AdditionalMonths) : IRequest<EnrollmentResponse>;
}
