using FluentValidation;
using MediatR;

namespace ProjectScheduleTraining.Application.Common;

/// <summary>
/// Behavior responsável por interceptar todos os commands e queries do MediatR
/// e executar as validações do FluentValidation antes de chegar ao handler.
///
/// Usa ValidateAsync para suportar validators com acesso assíncrono ao banco de dados
/// (ex: verificar duplicidade de CPF, e-mail já cadastrado, etc.).
/// Todos os validators são executados em paralelo via Task.WhenAll para menor latência.
/// </summary>
public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// Intercepta a requisição, executa todos os validators registrados de forma assíncrona
    /// e lança ValidationException caso existam falhas.
    /// </summary>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        // Executa todos os validators em paralelo — incluindo os assíncronos (ex: acesso ao banco)
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next();
    }
}
