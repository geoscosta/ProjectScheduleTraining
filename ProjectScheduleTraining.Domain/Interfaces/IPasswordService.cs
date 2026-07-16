namespace ProjectScheduleTraining.Domain.Interfaces
{
    /// <summary>
    /// Interface responsável por definir o contrato do serviço de senhas.
    /// Abstrai a criação e verificação de hashes de senhas.
    /// </summary>
    public interface IPasswordService
    {
        /// <summary>
        /// Gera o hash de uma senha em texto puro.
        /// </summary>
        string HashPassword(string password);

        /// <summary>
        /// Verifica se uma senha em texto puro corresponde ao hash armazenado.
        /// </summary>
        bool VerifyPassword(string password, string passwordHash);
    }
}
