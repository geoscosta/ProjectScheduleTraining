using ProjectScheduleTraining.Domain.Common;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        /// Dia de vencimento restrito a 5, 10, 15 ou 20 conforme contrato.
        public PaymentDueDay PaymentDueDay { get; set; }

        /// Método de pagamento afeta o desconto nos planos fidelidade.
        public PaymentMethod PaymentMethod { get; set; }

        /// Desconto aplicado (calculado automaticamente pelo tipo de plano + método).
        public decimal DiscountPercentage { get; set; }

        /// Valor final com desconto aplicado.
        public decimal FinalPrice { get; set; }

        public bool IsActive { get; set; }

        // Navegação
        public Student? Student { get; set; }
        public Plan? Plan { get; set; }
    }
}
