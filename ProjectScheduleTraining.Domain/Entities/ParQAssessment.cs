using ProjectScheduleTraining.Domain.Common;

namespace ProjectScheduleTraining.Domain.Entities
{
    /// <summary>
    /// Entidade responsável por armazenar a avaliação PAR-Q do aluno.
    /// O PAR-Q (Physical Activity Readiness Questionnaire) é obrigatório
    /// antes do início das atividades físicas.
    /// </summary>
    public class ParQAssessment : BaseEntity
    {
        public Guid StudentId { get; set; }
        public DateTime AssessmentDate { get; set; }

        /// Questão 1: Médico já disse que você possui algum problema cardíaco?
        public bool Question1 { get; set; }

        /// Questão 2: Você sente dor no peito quando pratica atividade física?
        public bool Question2 { get; set; }

        /// Questão 3: Você sentiu dor no peito no último mês sem praticar atividade?
        public bool Question3 { get; set; }

        /// Questão 4: Você perde o equilíbrio por tontura ou já perdeu a consciência?
        public bool Question4 { get; set; }

        /// Questão 5: Você possui algum problema ósseo ou articular?
        public bool Question5 { get; set; }

        /// Questão 6: Médico já receitou medicamento para pressão ou coração?
        public bool Question6 { get; set; }

        /// Questão 7: Você conhece alguma razão para não praticar atividade física?
        public bool Question7 { get; set; }

        /// Indica se o aluno foi liberado para praticar atividades físicas.
        /// False se qualquer resposta for verdadeira — requer avaliação médica.
        public bool IsCleared { get; set; }

        /// Observações adicionais do avaliador.
        public string? Notes { get; set; }

        // Navegação
        public Student? Student { get; set; }
    }
}
