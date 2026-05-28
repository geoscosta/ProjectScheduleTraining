using ProjectScheduleTraining.Domain.Common;

namespace ProjectScheduleTraining.Domain.Entities
{
    /// <summary>
    /// Entidade responsável por armazenar os exercícios de um treino.
    /// Cada exercício pertence a um treino e possui suas configurações específicas.
    /// </summary>
    public class WorkoutExercise : BaseEntity
    {
        public Guid WorkoutId { get; set; }

        /// Nome do exercício.
        public string Name { get; set; } = string.Empty;

        /// Grupo muscular trabalhado.
        public string MuscleGroup { get; set; } = string.Empty;

        /// Número de séries.
        public int Sets { get; set; }

        /// Número de repetições por série.
        public string Repetitions { get; set; } = string.Empty;

        /// Carga em kg.
        public decimal? Load { get; set; }

        /// Tempo de descanso em segundos.
        public int? RestSeconds { get; set; }

        /// Observações técnicas do exercício.
        public string? Notes { get; set; }

        /// Ordem de execução no treino.
        public int Order { get; set; }

        /// URL do vídeo demonstrativo (opcional).
        public string? VideoUrl { get; set; }

        // Navegação
        public StudentWorkout? Workout { get; set; }
    }
}
