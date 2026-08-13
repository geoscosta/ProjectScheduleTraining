namespace ProjectScheduleTraining.Domain.Enums;

/// <summary>
/// Enum responsável por representar a opção escolhida pelo aluno
/// no momento do cancelamento de plano fidelidade.
/// </summary>
public enum CancellationOption
{
    /// Paga multa de 20% sobre o saldo remanescente.
    PayPenalty = 1,

    /// Indica outra pessoa para ocupar a vaga (sem multa).
    SubstituteStudent = 2
}