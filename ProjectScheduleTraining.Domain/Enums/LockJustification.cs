namespace ProjectScheduleTraining.Domain.Enums
{
    /// <summary>
    /// Enum responsável por representar as justificativas válidas
    /// para trancamento de agenda e reposição de aulas.
    /// </summary>
    public enum LockJustification
    {
        /// Atestado médico.
        MedicalCertificate = 1,

        /// Afastamento por férias.
        Vacation = 2,

        /// Afastamento por trabalho.
        Work = 3,

        /// Problema de saúde sem atestado.
        Health = 4
    }
}
