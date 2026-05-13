using ProjectScheduleTraining.Domain.Common;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Domain.Entities
{
    public class Scheduling : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid ScheduleId { get; set; }
        public SchedulingStatus Status { get; set; }
        public bool IsMakeup { get; set; }
        public string? JustifiedAbsenceReason { get; set; }
        public string? TrainerNotes { get; set; }

        // Navegação
        public Student? Student { get; set; }
        public Schedule? Schedule { get; set; }
    }
}
