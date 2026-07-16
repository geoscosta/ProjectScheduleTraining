using Bogus;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Tests.Builders
{
    /// <summary>
    /// Builder responsável por criar instâncias de User
    /// com dados faker para uso nos testes unitários.
    /// </summary>
    public class UserBuilder
    {
        private readonly Faker _faker = new("pt_BR");
        private string _name;
        private string _email;
        private string _passwordHash;
        private UserRole _role;
        private bool _isActive;

        public UserBuilder()
        {
            _name = _faker.Name.FullName();
            _email = _faker.Internet.Email();
            _passwordHash = "hashedpassword";
            _role = UserRole.Admin;
            _isActive = true;
        }

        /// <summary>
        /// Define o e-mail do usuário.
        /// </summary>
        public UserBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        /// <summary>
        /// Define o hash da senha do usuário.
        /// </summary>
        public UserBuilder WithPasswordHash(string passwordHash)
        {
            _passwordHash = passwordHash;
            return this;
        }

        /// <summary>
        /// Define o perfil do usuário.
        /// </summary>
        public UserBuilder WithRole(UserRole role)
        {
            _role = role;
            return this;
        }

        /// <summary>
        /// Define se o usuário está ativo.
        /// </summary>
        public UserBuilder WithIsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        /// <summary>
        /// Constrói e retorna a instância do usuário com os dados configurados.
        /// </summary>
        public User Build()
        {
            return new User
            {
                Name = _name,
                Email = _email,
                PasswordHash = _passwordHash,
                Role = _role,
                IsActive = _isActive
            };
        }
    }
}
