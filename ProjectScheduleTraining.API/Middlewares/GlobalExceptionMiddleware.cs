using FluentValidation;
using ProjectScheduleTraining.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace ProjectScheduleTraining.API.Middlewares
{
    /// <summary>
    /// Middleware responsável por interceptar todas as exceções não tratadas da aplicação
    /// e retornar respostas padronizadas ao cliente.
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Intercepta a requisição e captura exceções não tratadas.
        /// Mapeia cada tipo de exceção para o status HTTP correspondente.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => e.ErrorMessage);
                await WriteResponseAsync(context, HttpStatusCode.BadRequest, errors);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(
                    "DomainException: {Code} — {Message}",
                    ex.Code,
                    ex.Message);

                await WriteResponseAsync(
                    context,
                    HttpStatusCode.UnprocessableEntity,
                    [ex.Message]);
            }
            catch (KeyNotFoundException ex)
            {
                await WriteResponseAsync(
                    context,
                    HttpStatusCode.NotFound,
                    [ex.Message]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno não tratado.");

                await WriteResponseAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    ["Ocorreu um erro interno. Tente novamente mais tarde."]);
            }
        }

        /// <summary>
        /// Escreve a resposta HTTP padronizada em formato JSON.
        /// </summary>
        private static async Task WriteResponseAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            IEnumerable<string> errors)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var body = JsonSerializer.Serialize(new
            {
                success = false,
                errors,
                timestamp = DateTime.UtcNow
            },
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(body);
        }
    }
}
