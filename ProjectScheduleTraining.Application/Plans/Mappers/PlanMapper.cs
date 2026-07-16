using ProjectScheduleTraining.Application.Plans.DTOs;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Application.Plans.Mappers
{
    /// <summary>
    /// Responsável por mapear a entidade Plan para os DTOs de resposta.
    /// Centraliza toda a lógica de mapeamento do módulo de planos.
    /// </summary>
    public static class PlanMapper
    {
        /// <summary>
        /// Mapeia a entidade Plan para o DTO de resposta completa.
        /// </summary>
        public static PlanResponse ToResponse(Plan plan)
            => new(
                plan.Id,
                plan.Name,
                plan.Type,
                plan.WeeklyFrequency,
                plan.DurationMonths,
                plan.Price,
                plan.IsActive,
                plan.Description,
                plan.CreatedAt,
                plan.UpdatedAt);

        /// <summary>
        /// Mapeia a entidade Plan para o DTO de resposta resumida.
        /// Utilizado em listagens onde não são necessários todos os dados.
        /// </summary>
        public static PlanSummaryResponse ToSummaryResponse(Plan plan)
            => new(
                plan.Id,
                plan.Name,
                plan.Type,
                plan.WeeklyFrequency,
                plan.DurationMonths,
                plan.Price);
    }
}
