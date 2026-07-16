using ProjectScheduleTraining.Application.Enrollments.DTOs;
using ProjectScheduleTraining.Application.Plans.Mappers;
using ProjectScheduleTraining.Application.Students.Mappers;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Application.Enrollments.Mappers
{
    /// <summary>
    /// Responsável por mapear a entidade Enrollment para os DTOs de resposta.
    /// Centraliza toda a lógica de mapeamento do módulo de matrículas.
    /// </summary>
    public static class EnrollmentMapper
    {
        /// <summary>
        /// Mapeia a entidade Enrollment para o DTO de resposta completa.
        /// Inclui os dados resumidos do aluno e do plano quando disponíveis.
        /// </summary>
        public static EnrollmentResponse ToResponse(Enrollment enrollment)
            => new(
                enrollment.Id,
                enrollment.StudentId,
                enrollment.PlanId,
                enrollment.StartDate,
                enrollment.ExpirationDate,
                enrollment.PaymentDueDay,
                enrollment.IsActive,
                enrollment.CreatedAt,
                enrollment.UpdatedAt,
                enrollment.Student is not null
                    ? StudentMapper.ToSummaryResponse(enrollment.Student)
                    : null,
                enrollment.Plan is not null
                    ? PlanMapper.ToSummaryResponse(enrollment.Plan)
                    : null);

        /// <summary>
        /// Mapeia a entidade Enrollment para o DTO de resposta resumida.
        /// Utilizado em listagens onde não são necessários todos os dados.
        /// </summary>
        public static EnrollmentSummaryResponse ToSummaryResponse(Enrollment enrollment)
            => new(
                enrollment.Id,
                enrollment.StudentId,
                enrollment.PlanId,
                enrollment.StartDate,
                enrollment.ExpirationDate,
                enrollment.IsActive);
    }
}
