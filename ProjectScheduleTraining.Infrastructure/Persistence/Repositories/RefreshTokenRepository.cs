using Microsoft.EntityFrameworkCore;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repositório responsável pelas operações de persistência do refresh token.
    /// </summary>
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Busca um refresh token pelo seu valor incluindo os dados do usuário.
        /// Utilizado no processo de renovação do access token.
        /// </summary>
        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);

        /// <summary>
        /// Revoga todos os refresh tokens ativos de um usuário.
        /// Utilizado no processo de logout.
        /// </summary>
        public async Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var tokens = await _dbSet
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ToListAsync(cancellationToken);

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }
        }
    }
}
