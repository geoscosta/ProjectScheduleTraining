using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Enrollments.DTOs;

/// <summary>
/// DTO de requisição para cancelamento de matrícula.
/// Para planos fidelidade é obrigatório informar a opção de cancelamento.
/// </summary>
public record CancelEnrollmentRequest(
    CancellationOption? CancellationOption,
    Guid? SubstituteStudentId);