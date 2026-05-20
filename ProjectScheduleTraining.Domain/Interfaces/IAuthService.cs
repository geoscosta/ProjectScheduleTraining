using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Domain.Interfaces
{
    /// <summary>
    /// Interface responsável por definir o contrato do serviço de autenticação.
    /// Abstrai a geração e validação de tokens JWT.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Gera um access token JWT para o usuário informado.
        /// </summary>
        string GenerateAccessToken(User user);

        /// <summary>
        /// Gera um refresh token aleatório e seguro.
        /// </summary>
        string GenerateRefreshToken();

        /// <summary>
        /// Retorna o tempo de expiração do access token em minutos.
        /// </summary>
        int GetAccessTokenExpirationMinutes();

        /// <summary>
        /// Retorna o tempo de expiração do refresh token em dias.
        /// </summary>
        int GetRefreshTokenExpirationDays();
    }
}
