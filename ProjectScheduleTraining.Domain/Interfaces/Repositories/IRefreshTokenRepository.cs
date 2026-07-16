using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface responsável por definir o contrato do repositório de refresh tokens.
    /// </summary>
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        /// <summary>
        /// Busca um refresh token pelo seu valor incluindo os dados do usuário.
        /// Utilizado no processo de renovação do access token.
        /// </summary>
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

        /// <summary>
        /// Revoga todos os refresh tokens ativos de um usuário.
        /// Utilizado no processo de logout.
        /// </summary>
        Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
