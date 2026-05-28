using ProjectScheduleTraining.Domain.Common;

namespace ProjectScheduleTraining.Domain.Entities
{
    /// <summary>
    /// Entidade responsável por armazenar os treinos personalizados dos alunos.
    /// Cada treino é criado pelo professor e visualizado pelo aluno no seu perfil.
    /// </summary>
    public class StudentWorkout : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid TrainerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        // Navegação
        public Student? Student { get; set; }
        public User? Trainer { get; set; }
        public ICollection<WorkoutExercise> Exercises { get; set; } = new List<WorkoutExercise>();
    }
}
