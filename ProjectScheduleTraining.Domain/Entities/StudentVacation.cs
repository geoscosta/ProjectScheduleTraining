using ProjectScheduleTraining.Domain.Common;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Domain.Entities;

/// <summary>
/// Entidade responsável por controlar os pedidos de férias dos alunos.
/// Conforme contrato: férias de 1 mês (pagamento normal + aviso 30 dias)
/// ou férias de 15 dias (pagamento de 50% da mensalidade).
/// </summary>
public class StudentVacation : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid EnrollmentId { get; set; }

    /// Tipo de férias solicitado.
    public VacationType VacationType { get; set; }

    /// Data de início das férias.
    public DateTime StartDate { get; set; }

    /// Data de retorno prevista.
    public DateTime EndDate { get; set; }

    /// Valor da mensalidade a ser cobrado no período (100% ou 50%).
    public decimal BillingAmount { get; set; }

    /// Indica se as férias foram aprovadas.
    public bool IsApproved { get; set; }

    /// Observações.
    public string? Notes { get; set; }

    // Navegação
    public Student? Student { get; set; }
    public Enrollment? Enrollment { get; set; }
}