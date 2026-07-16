using ProjectScheduleTraining.Domain.Common;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Domain.Entities
{
    public class Schedule : BaseEntity
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int MaxCapacity { get; set; }
        public int OccupiedSlots { get; set; }
        public ScheduleStatus Status { get; set; }
        public string? Notes { get; set; }

        // Navegação
        public ICollection<Scheduling> Schedulings { get; set; } = new List<Scheduling>();
    }
}
