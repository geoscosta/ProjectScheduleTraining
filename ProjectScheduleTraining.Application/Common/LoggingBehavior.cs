using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ProjectScheduleTraining.Application.Common
{
    /// <summary>
    /// Behavior responsável por registrar logs de entrada e saída
    /// de todos os commands e queries processados pelo MediatR.
    /// Registra também o tempo de execução de cada requisição.
    /// </summary>
    public class LoggingBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Intercepta a requisição, registra o início da execução,
        /// aguarda o próximo behavior e registra o tempo total de execução.
        /// Em caso de erro, registra o log de falha com os detalhes da exceção.
        /// </summary>
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation(
                "Iniciando execução de {RequestName}",
                requestName);

            try
            {
                var response = await next();

                stopwatch.Stop();

                _logger.LogInformation(
                    "Execução de {RequestName} concluída em {ElapsedMilliseconds}ms",
                    requestName,
                    stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(
                    ex,
                    "Erro na execução de {RequestName} após {ElapsedMilliseconds}ms",
                    requestName,
                    stopwatch.ElapsedMilliseconds);

                throw;
            }
        }
    }
}
