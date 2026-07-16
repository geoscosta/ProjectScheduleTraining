using ProjectScheduleTraining.Domain.Common;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Domain.Entities
{
    /// <summary>
    /// Entidade responsável por representar um usuário do sistema.
    /// Cada usuário possui um perfil que define suas permissões de acesso.
    /// </summary>
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public Guid? StudentId { get; set; }

        // Navegação
        public Student? Student { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
