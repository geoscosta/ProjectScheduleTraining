using ProjectScheduleTraining.Domain.Common;
using ProjectScheduleTraining.Domain.Exceptions;

namespace ProjectScheduleTraining.Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int PaymentDueDay { get; set; }
        public bool IsActive { get; set; }

        // Navegação
        public Student? Student { get; set; }
        public Plan? Plan { get; set; }
    }
}
