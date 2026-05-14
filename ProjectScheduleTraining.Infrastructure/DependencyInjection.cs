using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;
using ProjectScheduleTraining.Infrastructure.Persistence.Repositories;

namespace ProjectScheduleTraining.Infrastructure
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registra todos os serviços da camada Infrastructure no container de injeção de dependência.
        /// Inclui contexto do banco de dados, repositórios e Unit of Work.
        /// </summary>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddDatabase(configuration)
                .AddRepositories();

            return services;
        }

        /// <summary>
        /// Configura o contexto do banco de dados com SQL Server.
        /// A connection string é lida a partir do arquivo de configuração.
        /// </summary>
        private static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsAssembly(
                        typeof(AppDbContext).Assembly.FullName)));

            return services;
        }

        /// <summary>
        /// Registra todos os repositórios e o Unit of Work no container de DI.
        /// Todos são registrados com ciclo de vida Scoped,
        /// garantindo uma instância por requisição HTTP.
        /// </summary>
        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<ISchedulingRepository, SchedulingRepository>();
            services.AddScoped<IFinancialRepository, FinancialRepository>();

            return services;
        }
    }
}
