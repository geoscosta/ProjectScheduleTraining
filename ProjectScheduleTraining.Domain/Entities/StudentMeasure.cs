using ProjectScheduleTraining.Domain.Common;

namespace ProjectScheduleTraining.Domain.Entities
{
    /// <summary>
    /// Entidade responsável por armazenar as medidas corporais do aluno.
    /// Cada registro representa uma avaliação física em uma data específica.
    /// </summary>
    public class StudentMeasure : BaseEntity
    {
        public Guid StudentId { get; set; }
        public DateTime MeasureDate { get; set; }

        /// Dados antropométricos básicos.
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal Bmi { get; set; }

        /// Circunferências em centímetros.
        public decimal? ChestCircumference { get; set; }
        public decimal? WaistCircumference { get; set; }
        public decimal? HipCircumference { get; set; }
        public decimal? ArmCircumference { get; set; }
        public decimal? ThighCircumference { get; set; }
        public decimal? CalfCircumference { get; set; }

        /// Percentual de gordura e massa magra.
        public decimal? BodyFatPercentage { get; set; }
        public decimal? LeanMassPercentage { get; set; }

        /// Observações do avaliador.
        public string? Notes { get; set; }

        // Navegação
        public Student? Student { get; set; }
    }
}
