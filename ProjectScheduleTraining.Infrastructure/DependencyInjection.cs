using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProjectScheduleTraining.Domain.Interfaces;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;
using ProjectScheduleTraining.Infrastructure.Persistence.Repositories;
using ProjectScheduleTraining.Infrastructure.Security;
using System.Text;

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
                .AddRepositories()
                .AddSecurity(configuration)
                .AddJwtAuthentication(configuration);

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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IStudentMeasureRepository, StudentMeasureRepository>();
            services.AddScoped<IParQAssessmentRepository, ParQAssessmentRepository>();
            services.AddScoped<IStudentContractRepository, StudentContractRepository>();
            services.AddScoped<IStudentWorkoutRepository, StudentWorkoutRepository>();
            services.AddScoped<IScheduleLockRepository, ScheduleLockRepository>();

            return services;
        }

        /// <summary>
        /// Registra os serviços de segurança no container de DI.
        /// Inclui o serviço de autenticação JWT e o serviço de senhas.
        /// </summary>
        private static IServiceCollection AddSecurity(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtSettings>(
                configuration.GetSection("Jwt"));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPasswordService, PasswordService>();

            return services;
        }

        /// <summary>
        /// Configura a autenticação JWT no pipeline do ASP.NET Core.
        /// Define as regras de validação do token.
        /// </summary>
        private static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()!;

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("TrainerOrAdmin", policy =>
                    policy.RequireRole("Admin", "Trainer"));

                options.AddPolicy("ReceptionistOrAdmin", policy =>
                    policy.RequireRole("Admin", "Receptionist"));

                options.AddPolicy("AllRoles", policy =>
                    policy.RequireRole("Admin", "Trainer", "Receptionist", "Student"));
            });

            return services;
        }
    }
}
