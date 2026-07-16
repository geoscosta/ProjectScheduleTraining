using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProjectScheduleTraining.Application.Common;

namespace ProjectScheduleTraining.Application
{
    /// <summary>
    /// Responsável por registrar todos os serviços da camada Application
    /// no container de injeção de dependência.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registra os serviços da camada Application no container de DI.
        /// Inclui MediatR, FluentValidation e Pipeline Behaviors.
        /// </summary>
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services
                .AddMediatR()
                .AddValidators()
                .AddBehaviors();

            return services;
        }

        /// <summary>
        /// Registra o MediatR e todos os handlers do assembly Application.
        /// </summary>
        private static IServiceCollection AddMediatR(
            this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(DependencyInjection).Assembly));

            return services;
        }

        /// <summary>
        /// Registra todos os validators do FluentValidation
        /// presentes no assembly Application.
        /// </summary>
        private static IServiceCollection AddValidators(
            this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(
                typeof(DependencyInjection).Assembly);

            return services;
        }

        /// <summary>
        /// Registra os pipeline behaviors do MediatR na ordem correta de execução.
        /// Ordem: LoggingBehavior → ValidationBehavior → Handler.
        /// </summary>
        private static IServiceCollection AddBehaviors(
            this IServiceCollection services)
        {
            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(LoggingBehavior<,>));

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
