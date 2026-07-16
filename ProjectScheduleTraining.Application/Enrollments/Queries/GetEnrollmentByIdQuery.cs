using MediatR;
using ProjectScheduleTraining.Application.Enrollments.DTOs;

namespace ProjectScheduleTraining.Application.Enrollments.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para busca de uma matrícula pelo seu identificador único.
    /// </summary>
    public record GetEnrollmentByIdQuery(Guid Id) : IRequest<EnrollmentResponse>;
}
