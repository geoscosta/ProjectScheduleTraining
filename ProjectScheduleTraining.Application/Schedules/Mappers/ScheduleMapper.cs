using ProjectScheduleTraining.Application.Schedules.DTOs;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Application.Schedules.Mappers
{
    /// <summary>
    /// Responsável por mapear a entidade Schedule para os DTOs de resposta.
    /// Centraliza toda a lógica de mapeamento do módulo de horários.
    /// </summary>
    public static class ScheduleMapper
    {
        /// <summary>
        /// Mapeia a entidade Schedule para o DTO de resposta completa.
        /// Calcula as vagas disponíveis subtraindo as ocupadas da capacidade máxima.
        /// </summary>
        public static ScheduleResponse ToResponse(Schedule schedule)
            => new(
                schedule.Id,
                schedule.Date,
                schedule.StartTime,
                schedule.EndTime,
                schedule.MaxCapacity,
                schedule.OccupiedSlots,
                schedule.MaxCapacity - schedule.OccupiedSlots,
                schedule.Status,
                schedule.Notes,
                schedule.CreatedAt,
                schedule.UpdatedAt);

        /// <summary>
        /// Mapeia a entidade Schedule para o DTO de resposta resumida.
        /// Utilizado em listagens e visualização de agenda.
        /// </summary>
        public static ScheduleSummaryResponse ToSummaryResponse(Schedule schedule)
            => new(
                schedule.Id,
                schedule.Date,
                schedule.StartTime,
                schedule.EndTime,
                schedule.MaxCapacity - schedule.OccupiedSlots,
                schedule.Status);
    }
}
