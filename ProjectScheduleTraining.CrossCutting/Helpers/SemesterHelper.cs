using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.CrossCutting.Helpers
{
    /// <summary>
    /// Helper responsável por calcular o semestre atual e validar
    /// se um aluno já utilizou o trancamento no semestre vigente.
    /// </summary>
    public static class SemesterHelper
    {
        /// <summary>
        /// Retorna o semestre correspondente ao mês informado.
        /// Primeiro semestre: Janeiro (1) a Junho (6).
        /// Segundo semestre: Julho (7) a Dezembro (12).
        /// </summary>
        public static Semester GetSemester(DateTime date)
            => date.Month <= 6 ? Semester.First : Semester.Second;

        /// <summary>
        /// Retorna o semestre atual baseado na data do sistema.
        /// </summary>
        public static Semester GetCurrentSemester()
            => GetSemester(DateTime.UtcNow);

        /// <summary>
        /// Retorna o ano atual.
        /// </summary>
        public static int GetCurrentYear()
            => DateTime.UtcNow.Year;

        /// <summary>
        /// Retorna a descrição formatada do semestre para exibição.
        /// Exemplo: "1º Semestre/2026"
        /// </summary>
        public static string GetSemesterLabel(Semester semester, int year)
            => semester == Semester.First
                ? $"1º Semestre/{year}"
                : $"2º Semestre/{year}";
    }
}
