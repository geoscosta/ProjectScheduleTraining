using ProjectScheduleTraining.Application.Schedules.Mappers;
using ProjectScheduleTraining.Application.Schedulings.DTOs;
using ProjectScheduleTraining.Application.Students.Mappers;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Application.Schedulings.Mappers
{
    /// <summary>
    /// Responsável por mapear a entidade Scheduling para os DTOs de resposta.
    /// Centraliza toda a lógica de mapeamento do módulo de agendamentos.
    /// </summary>
    public static class SchedulingMapper
    {
        /// <summary>
        /// Mapeia a entidade Scheduling para o DTO de resposta completa.
        /// Inclui os dados resumidos do aluno e do horário quando disponíveis.
        /// </summary>
        public static SchedulingResponse ToResponse(Scheduling scheduling)
            => new(
                scheduling.Id,
                scheduling.StudentId,
                scheduling.ScheduleId,
                scheduling.Status,
                scheduling.IsMakeup,
                scheduling.JustifiedAbsenceReason,
                scheduling.TrainerNotes,
                scheduling.CreatedAt,
                scheduling.UpdatedAt,
                scheduling.Student is not null
                    ? StudentMapper.ToSummaryResponse(scheduling.Student)
                    : null,
                scheduling.Schedule is not null
                    ? ScheduleMapper.ToSummaryResponse(scheduling.Schedule)
                    : null);

        /// <summary>
        /// Mapeia a entidade Scheduling para o DTO de resposta resumida.
        /// Utilizado em listagens onde não são necessários todos os dados.
        /// </summary>
        public static SchedulingSummaryResponse ToSummaryResponse(Scheduling scheduling)
            => new(
                scheduling.Id,
                scheduling.StudentId,
                scheduling.ScheduleId,
                scheduling.Status,
                scheduling.IsMakeup);
    }
}
