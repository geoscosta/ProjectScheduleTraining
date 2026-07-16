using Microsoft.EntityFrameworkCore;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repositório responsável pelas operações de persistência do usuário.
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Busca um usuário pelo e-mail.
        /// Retorna null caso não seja encontrado.
        /// </summary>
        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(
                x => x.Email == email.ToLower(),
                cancellationToken);

        /// <summary>
        /// Busca um usuário pelo e-mail incluindo os refresh tokens ativos.
        /// Utilizado no processo de login e renovação de token.
        /// </summary>
        public async Task<User?> GetByEmailWithTokensAsync(string email, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(
                    x => x.Email == email.ToLower(),
                    cancellationToken);

        /// <summary>
        /// Verifica se já existe um usuário cadastrado com o e-mail informado.
        /// </summary>
        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
            => await _dbSet.AnyAsync(
                x => x.Email == email.ToLower(),
                cancellationToken);
    }
}
