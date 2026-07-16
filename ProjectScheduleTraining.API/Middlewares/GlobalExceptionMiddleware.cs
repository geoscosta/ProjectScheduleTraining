using FluentValidation;
using ProjectScheduleTraining.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace ProjectScheduleTraining.API.Middlewares;

/// <summary>
/// Middleware responsável por interceptar todas as exceções não tratadas da aplicação
/// e retornar respostas padronizadas ao cliente com o status HTTP semanticamente correto.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    // Códigos de erro de domínio que representam conflito de estado (409)
    private static readonly HashSet<string> ConflictCodes =
    [
        "ENROLLMENT_ALREADY_EXISTS",
        "SCHEDULING_ALREADY_EXISTS",
        "SCHEDULE_LOCK_ALREADY_ACTIVE",
        "SCHEDULE_LOCK_ALREADY_USED",
        "USER_EMAIL_ALREADY_EXISTS",
        "SCHEDULING_ALREADY_CANCELLED",
        "SCHEDULING_ALREADY_PRESENT"
    ];

    // Códigos de erro de domínio que representam recurso não encontrado (404)
    private static readonly HashSet<string> NotFoundCodes =
    [
        "STUDENT_NOT_FOUND",
        "PLAN_NOT_FOUND",
        "ENROLLMENT_NOT_FOUND",
        "SCHEDULE_NOT_FOUND",
        "SCHEDULING_NOT_FOUND",
        "FINANCIAL_NOT_FOUND",
        "SCHEDULE_LOCK_NOT_FOUND",
        "USER_NOT_FOUND"
    ];

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

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
                "DomainException: {Code} | {Message}",
                ex.Code,
                ex.Message);

            var statusCode = ResolveStatusCode(ex.Code);
            await WriteResponseAsync(context, statusCode, [ex.Message], ex.Code);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteResponseAsync(context, HttpStatusCode.NotFound, [ex.Message]);
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
    /// Mapeia o código de erro do domínio para o status HTTP semanticamente correto.
    ///
    /// - 404 Not Found: recurso solicitado não existe no sistema
    /// - 409 Conflict: operação viola o estado atual do recurso (duplicata, já processado)
    /// - 422 Unprocessable Entity: requisição válida mas viola regra de negócio
    /// </summary>
    private static HttpStatusCode ResolveStatusCode(string code)
    {
        if (NotFoundCodes.Contains(code))
            return HttpStatusCode.NotFound;

        if (ConflictCodes.Contains(code))
            return HttpStatusCode.Conflict;

        return HttpStatusCode.UnprocessableEntity;
    }

    private static async Task WriteResponseAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        IEnumerable<string> errors,
        string? errorCode = null)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var body = JsonSerializer.Serialize(new
        {
            success = false,
            errorCode,
            errors,
            timestamp = DateTime.UtcNow
        },
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        await context.Response.WriteAsync(body);
    }
}
