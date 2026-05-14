using MediatR;
using ProjectScheduleTraining.Application.Students.DTOs;

namespace ProjectScheduleTraining.Application.Students.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para busca de um aluno pelo seu identificador único.
    /// </summary>
    public record GetStudentByIdQuery(Guid Id) : IRequest<StudentResponse>;
}
