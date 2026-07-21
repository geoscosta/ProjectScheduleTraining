using Bogus;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Tests.Builders
{
    /// <summary>
    /// Builder responsável por criar instâncias de Plan
    /// com dados faker para uso nos testes unitários.
    /// </summary>
    public class PlanBuilder
    {
        private readonly Faker _faker = new("pt_BR");
        private string _name;
        private PlanType _type;
        private WeeklyFrequency _weeklyFrequency;
        private int _durationMonths;
        private decimal _price;
        private bool _isActive;

        public PlanBuilder()
        {
            _name = _faker.Commerce.ProductName();
            _type = PlanType.Monthly;
            _weeklyFrequency = WeeklyFrequency.ThreeTimesAWeek;
            _durationMonths = 1;
            _price = _faker.Random.Decimal(100, 500);
            _isActive = true;
        }

        /// <summary>
        /// Define o nome do plano.
        /// </summary>
        public PlanBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// Define o preço do plano.
        /// </summary>
        public PlanBuilder WithPrice(decimal price)
        {
            _price = price;
            return this;
        }

        /// <summary>
        /// Define se o plano está ativo.
        /// </summary>
        public PlanBuilder WithIsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        /// <summary>
        /// Define o tipo do plano e ajusta a duração automaticamente.
        /// </summary>
        public PlanBuilder WithType(PlanType type)
        {
            _type = type;

            /// Ajusta a duração automaticamente conforme o tipo do plano.
            _durationMonths = type switch
            {
                PlanType.Quarterly => 3,
                PlanType.SemiAnnual => 6,
                PlanType.Annual => 12,
                _ => 1
            };

            return this;
        }

        /// <summary>
        /// Define a frequência semanal do plano.
        /// </summary>
        public PlanBuilder WithWeeklyFrequency(WeeklyFrequency frequency)
        {
            _weeklyFrequency = frequency;
            return this;
        }

        /// <summary>
        /// Constrói e retorna a instância do plano com os dados configurados.
        /// </summary>
        public Plan Build()
        {
            return new Plan
            {
                Name = _name,
                Type = _type,
                WeeklyFrequency = _weeklyFrequency,
                DurationMonths = _durationMonths,
                Price = _price,
                IsActive = _isActive
            };
        }
    }
}
