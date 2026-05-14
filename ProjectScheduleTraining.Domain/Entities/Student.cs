using ProjectScheduleTraining.Domain.Common;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContact { get; set; }
        public string? InternalNotes { get; set; }
        public StudentStatus Status { get; set; }
        public DateTime StartDate { get; set; }

        // Navegação
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Scheduling> Schedulings { get; set; } = new List<Scheduling>();
        public ICollection<Financial> Financials { get; set; } = new List<Financial>();
    }
}
