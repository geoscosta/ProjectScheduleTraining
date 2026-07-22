using ProjectScheduleTraining.Domain.Common;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Domain.Entities
{
    public class Scheduling : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid ScheduleId { get; set; }
        public SchedulingStatus Status { get; set; }

        /// Indica se este agendamento é uma reposição.
        public bool IsMakeup { get; set; }

        /// Indica se a falta foi justificada com atestado médico.
        /// Atestados não contam no limite de 2 reposições por mês.
        public bool HasMedicalCertificate { get; set; }

        /// Data limite para realizar a reposição (30 dias a partir da falta).
        public DateTime? MakeupDeadline { get; set; }

        /// Justificativa da ausência para faltas justificadas.
        public string? JustifiedAbsenceReason { get; set; }

        /// Notas do professor no momento do check-in.
        public string? TrainerNotes { get; set; }

        // Navegação
        public Student? Student { get; set; }
        public Schedule? Schedule { get; set; }
    }
}
