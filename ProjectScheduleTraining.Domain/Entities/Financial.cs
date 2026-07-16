using ProjectScheduleTraining.Domain.Common;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Domain.Entities
{
    public class Financial : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid? EnrollmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public FinancialStatus Status { get; set; }
        public string? Description { get; set; }
        public string? PaymentProof { get; set; }

        // Navegação
        public Student? Student { get; set; }
        public Enrollment? Enrollment { get; set; }
    }
}
