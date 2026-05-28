using ProjectScheduleTraining.Domain.Common;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Domain.Entities
{
    /// <summary>
    /// Entidade responsável por armazenar os trancamentos de agenda dos alunos.
    /// Apenas alunos com planos fidelidade têm direito a trancamento.
    /// </summary>
    public class ScheduleLock : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid EnrollmentId { get; set; }

        /// Data de início do trancamento.
        public DateTime LockStartDate { get; set; }

        /// Data de fim do trancamento.
        public DateTime LockEndDate { get; set; }

        /// Justificativa do trancamento.
        public LockJustification Justification { get; set; }

        /// Documento comprobatório (URL do arquivo).
        public string? DocumentUrl { get; set; }

        /// Observações adicionais.
        public string? Notes { get; set; }

        /// Indica se o trancamento foi aprovado pelo administrador.
        public bool IsApproved { get; set; }

        /// Ano de referência do trancamento.
        public int Year { get; set; }

        /// Semestre de referência do trancamento.
        public Semester Semester { get; set; }

        // Navegação
        public Student? Student { get; set; }
        public Enrollment? Enrollment { get; set; }
    }
}
