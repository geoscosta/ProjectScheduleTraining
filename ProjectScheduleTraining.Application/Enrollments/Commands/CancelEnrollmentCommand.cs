using MediatR;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Enrollments.Commands;

/// <summary>
/// Command responsável por cancelar uma matrícula.
/// Para planos fidelidade, o aluno escolhe entre pagar multa
/// ou indicar um substituto para a vaga.
/// </summary>
public record CancelEnrollmentCommand(
    Guid Id,
    CancellationOption? CancellationOption,
    Guid? SubstituteStudentId) : IRequest;