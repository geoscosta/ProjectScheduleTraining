using ProjectScheduleTraining.Domain.Common;

namespace ProjectScheduleTraining.Domain.Entities;

/// <summary>
/// Entidade responsável por representar as entidades prestadoras de serviço.
/// Conforme novo contrato: Estúdio Trinca + Letícia Sales (dois CNPJs).
/// </summary>
public class Contractor : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
}