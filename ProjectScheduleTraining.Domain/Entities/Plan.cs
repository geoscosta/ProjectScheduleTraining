using ProjectScheduleTraining.Domain.Common;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Domain.Entities
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public PlanType Type { get; set; }
        public WeeklyFrequency WeeklyFrequency { get; set; }
        public int DurationMonths { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }
}
