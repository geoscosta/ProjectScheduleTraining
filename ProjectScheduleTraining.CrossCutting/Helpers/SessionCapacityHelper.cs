using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.CrossCutting.Helpers
{
    /// <summary>
    /// Helper responsável por calcular a capacidade máxima de alunos
    /// por tipo de sessão conforme definido no contrato do Estúdio Trinca.
    /// </summary>
    public static class SessionCapacityHelper
    {
        /// <summary>
        /// Retorna a capacidade máxima de alunos por tipo de sessão.
        /// Pilates: 3 · Semi Personalizado: 4 · Individualizado: 1
        /// </summary>
        public static int GetMaxCapacity(SessionType sessionType)
            => sessionType switch
            {
                SessionType.Pilates => 3,
                SessionType.SemiPersonalized => 4,
                SessionType.Individual => 1,
                _ => 4
            };

        /// <summary>
        /// Retorna a duração padrão de uma sessão em minutos.
        /// Contrato: 50 minutos por sessão.
        /// </summary>
        public static int GetSessionDurationMinutes() => 50;

        /// <summary>
        /// Calcula o horário de término com base no início e duração padrão.
        /// </summary>
        public static TimeSpan GetEndTime(TimeSpan startTime)
            => startTime.Add(TimeSpan.FromMinutes(GetSessionDurationMinutes()));
    }
}
