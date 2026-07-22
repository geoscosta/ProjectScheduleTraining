using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProjectScheduleTraining.Domain.Interfaces;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Jobs;
using ProjectScheduleTraining.Infrastructure.Persistence;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;
using ProjectScheduleTraining.Infrastructure.Security;
using System.Text;

namespace ProjectScheduleTraining.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registra todos os serviços da camada Infrastructure no container de injeção de dependência.
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDatabase(configuration)
            .AddUnitOfWork()
            .AddSecurity(configuration)
            .AddJwtAuthentication(configuration)
            .AddJobs();

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        return services;
    }

    /// <summary>
    /// Registra apenas o IUnitOfWork — ele já agrega e instancia todos os repositórios
    /// internamente, passando o mesmo AppDbContext para todos.
    ///
    /// Não registrar os repositórios individualmente evita a existência de duas instâncias
    /// distintas do mesmo repositório por request (uma via IUnitOfWork e outra via DI direto),
    /// o que causaria inconsistência de estado no ChangeTracker do EF Core.
    /// </summary>
    private static IServiceCollection AddUnitOfWork(
        this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    private static IServiceCollection AddSecurity(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordService, PasswordService>();
        return services;
    }

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

    /// <summary>
    /// Registra os background jobs da aplicação.
    /// AutoBlockOverdueStudentsJob: bloqueia alunos com cobranças
    /// vencidas há mais de 2 dias conforme Cláusula 32 do contrato.
    /// </summary>
    private static IServiceCollection AddJobs(
        this IServiceCollection services)
    {
        services.AddHostedService<AutoBlockOverdueStudentsJob>();
        return services;
    }
}
