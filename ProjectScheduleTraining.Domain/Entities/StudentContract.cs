using ProjectScheduleTraining.Domain.Common;

namespace ProjectScheduleTraining.Domain.Entities
{
    /// <summary>
    /// Entidade responsável por armazenar os contratos assinados digitalmente.
    /// Cada matrícula gera um contrato vinculado ao aluno.
    /// </summary>
    public class StudentContract : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid EnrollmentId { get; set; }
        public DateTime SignedAt { get; set; }

        /// Hash de verificação da assinatura digital.
        public string SignatureHash { get; set; } = string.Empty;

        /// IP do dispositivo no momento da assinatura.
        public string? SignatureIp { get; set; }

        /// Conteúdo do contrato em HTML no momento da assinatura.
        public string ContractContent { get; set; } = string.Empty;

        /// URL do PDF gerado após a assinatura.
        public string? PdfUrl { get; set; }

        /// Indica se o contrato foi assinado.
        public bool IsSigned { get; set; }

        // Navegação
        public Student? Student { get; set; }
        public Enrollment? Enrollment { get; set; }
    }
}
