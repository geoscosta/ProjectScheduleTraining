namespace ProjectScheduleTraining.Domain.Enums
{
    /// <summary>
    /// Enum responsável por representar o tipo de sessão de treino.
    /// Define a capacidade máxima de alunos por turma conforme contrato.
    /// </summary>
    public enum SessionType
    {
        /// Pilates — máximo 3 alunos por turma.
        Pilates = 1,

        /// Semi Personalizado — máximo 4 alunos por turma.
        SemiPersonalized = 2,

        /// Individualizado — máximo 1 aluno por turma.
        Individual = 3
    }
}
