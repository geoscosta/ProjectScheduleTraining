using ProjectScheduleTraining.API.Extensions;
using ProjectScheduleTraining.API.Middlewares;
using ProjectScheduleTraining.Application;
using ProjectScheduleTraining.Infrastructure;

namespace ProjectScheduleTraining.API;

/// <summary>
/// Responsável por registrar todos os serviços da camada API no container de DI
/// e configurar o pipeline de middlewares.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddApplication()
            .AddInfrastructure(configuration)
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy =
                    System.Text.Json.JsonNamingPolicy.CamelCase;
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerWithJwt();

        // Origins lidos do appsettings — sem hardcode no código-fonte.
        // Em dev: appsettings.Development.json (gitignored)
        // Em prod: variável de ambiente Cors__AllowedOrigins__0, Cors__AllowedOrigins__1...
        var allowedOrigins = configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

        services.AddCors(options =>
            options.AddPolicy("Frontend", policy =>
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()));

        return services;
    }

    public static WebApplication UseApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "Project Schedule Training API v1"));
        }

        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseHttpsRedirection();
        app.UseCors("Frontend");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}
