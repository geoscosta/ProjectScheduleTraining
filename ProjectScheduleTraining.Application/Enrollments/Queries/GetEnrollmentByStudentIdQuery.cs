using MediatR;
using ProjectScheduleTraining.Application.Enrollments.DTOs;

namespace ProjectScheduleTraining.Application.Enrollments.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para busca da matrícula ativa de um aluno.
    /// </summary>
    public record GetEnrollmentByStudentIdQuery(Guid StudentId) : IRequest<EnrollmentResponse>;
}
