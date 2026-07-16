using Bogus;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Tests.Builders
{
    /// <summary>
    /// Builder responsável por criar instâncias de Student
    /// com dados faker para uso nos testes unitários.
    /// </summary>
    public class StudentBuilder
    {
        private readonly Faker _faker = new("pt_BR");
        private string _name;
        private string _cpf;
        private string _email;
        private string _phone;
        private DateTime _birthDate;
        private StudentStatus _status;

        public StudentBuilder()
        {
            _name = _faker.Name.FullName();
            _cpf = "52998224725";
            _email = _faker.Internet.Email();
            _phone = _faker.Phone.PhoneNumber();
            _birthDate = _faker.Date.Past(30, DateTime.Today.AddYears(-18));
            _status = StudentStatus.Active;
        }

        /// <summary>
        /// Define o nome do aluno.
        /// </summary>
        public StudentBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// Define o CPF do aluno.
        /// </summary>
        public StudentBuilder WithCpf(string cpf)
        {
            _cpf = cpf;
            return this;
        }

        /// <summary>
        /// Define o e-mail do aluno.
        /// </summary>
        public StudentBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        /// <summary>
        /// Define o status do aluno.
        /// </summary>
        public StudentBuilder WithStatus(StudentStatus status)
        {
            _status = status;
            return this;
        }

        /// <summary>
        /// Constrói e retorna a instância do aluno com os dados configurados.
        /// </summary>
        public Student Build()
        {
            return new Student
            {
                Name = _name,
                Cpf = _cpf,
                Email = _email,
                Phone = _phone,
                BirthDate = _birthDate,
                Status = _status,
                StartDate = DateTime.UtcNow
            };
        }
    }
}
