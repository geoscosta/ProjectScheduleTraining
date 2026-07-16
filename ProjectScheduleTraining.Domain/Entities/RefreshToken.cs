using ProjectScheduleTraining.Domain.Common;

namespace ProjectScheduleTraining.Domain.Entities
{
    /// <summary>
    /// Entidade responsável por armazenar os refresh tokens dos usuários.
    /// Utilizado para renovação do access token sem necessidade de novo login.
    /// </summary>
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }

        // Navegação
        public User? User { get; set; }

        /// <summary>
        /// Verifica se o refresh token ainda é válido.
        /// </summary>
        public bool IsValid() => !IsRevoked && DateTime.UtcNow < ExpiresAt;
    }
}
