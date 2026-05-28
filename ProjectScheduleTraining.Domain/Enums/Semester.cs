namespace ProjectScheduleTraining.Domain.Enums
{
    /// <summary>
    /// Enum responsável por representar os semestres do ano.
    /// Utilizado no controle de trancamentos de agenda.
    /// Cada aluno tem direito a 1 trancamento por semestre.
    /// </summary>
    public enum Semester
    {
        /// Primeiro semestre — Janeiro a Junho.
        First = 1,

        /// Segundo semestre — Julho a Dezembro.
        Second = 2
    }
}
