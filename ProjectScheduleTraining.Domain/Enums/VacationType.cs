namespace ProjectScheduleTraining.Domain.Enums;

/// <summary>
/// Enum responsável por representar o tipo de férias solicitado pelo aluno.
/// </summary>
public enum VacationType
{
    /// Férias de 1 mês — pagamento normal, aviso com 30 dias de antecedência.
    OneMonth = 1,

    /// Férias de 15 dias — pagamento de 50% da mensalidade para garantir a vaga.
    FifteenDays = 2
}