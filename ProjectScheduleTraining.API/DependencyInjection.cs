using ProjectScheduleTraining.API.Extensions;
using ProjectScheduleTraining.API.Middlewares;
using ProjectScheduleTraining.Application;
using ProjectScheduleTraining.Infrastructure;

namespace ProjectScheduleTraining.API
{
    /// <summary>
    /// Responsável por registrar todos os serviços da camada API
    /// no container de injeção de dependência.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registra todos os serviços necessários para o funcionamento da API.
        /// Inclui serviços das camadas Application e Infrastructure.
        /// </summary>
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

            services.AddCors(options =>
                options.AddPolicy("Frontend", policy =>
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()));

            return services;
        }

        /// <summary>
        /// Configura o pipeline de middlewares da aplicação.
        /// Define a ordem correta de execução de cada middleware.
        /// </summary>
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
}
