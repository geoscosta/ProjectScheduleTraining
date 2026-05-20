using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface responsável por definir o contrato do repositório de usuários.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Busca um usuário pelo e-mail.
        /// Retorna null caso não seja encontrado.
        /// </summary>
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca um usuário pelo e-mail incluindo os refresh tokens ativos.
        /// Utilizado no processo de login e renovação de token.
        /// </summary>
        Task<User?> GetByEmailWithTokensAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifica se já existe um usuário cadastrado com o e-mail informado.
        /// </summary>
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    }
}
