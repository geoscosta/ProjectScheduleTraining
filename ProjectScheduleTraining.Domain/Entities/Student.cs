using ProjectScheduleTraining.Domain.Common;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Domain.Entities
{
    public class Student : BaseEntity
    {
        // Dados pessoais básicos.
        public string Name { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Profession { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public string? IdentityDocument { get; set; }

        // Endereço estruturado.
        public string? Street { get; set; }
        public string? AddressNumber { get; set; }
        public string? Complement { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }

        // Responsável legal (para menores de 18 anos).
        public string? GuardianName { get; set; }
        public string? GuardianCpf { get; set; }

        // Documentação e termos.
        public bool RegistrationFeePaid { get; set; }
        public DateTime? HealthCertificateExpiresAt { get; set; }
        public bool ImageRightsAccepted { get; set; }
        public bool InternalRegulationAccepted { get; set; }

        // Notas e status.
        public string? EmergencyContact { get; set; }
        public string? InternalNotes { get; set; }
        public StudentStatus Status { get; set; }
        public DateTime StartDate { get; set; }
       

        // Navegação
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Scheduling> Schedulings { get; set; } = new List<Scheduling>();
        public ICollection<Financial> Financials { get; set; } = new List<Financial>();
        public ICollection<StudentMeasure> Measures { get; set; } = new List<StudentMeasure>();
        public ICollection<ParQAssessment> ParQAssessments { get; set; } = new List<ParQAssessment>();
        public ICollection<StudentContract> Contracts { get; set; } = new List<StudentContract>();
        public ICollection<StudentWorkout> Workouts { get; set; } = new List<StudentWorkout>();
        public ICollection<ScheduleLock> ScheduleLocks { get; set; } = new List<ScheduleLock>();
    }
}
