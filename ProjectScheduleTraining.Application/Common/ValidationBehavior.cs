using FluentValidation;
using MediatR;

namespace ProjectScheduleTraining.Application.Common
{
    /// <summary>
    /// Behavior responsável por interceptar todos os commands e queries do MediatR
    /// e executar as validações do FluentValidation antes de chegar ao handler.
    /// Lança ValidationException caso alguma regra de validação seja violada.
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
        /// Intercepta a requisição, executa todos os validators registrados
        /// e lança exceção caso existam falhas de validação.
        /// Caso não existam validators registrados, segue para o próximo behavior.
        /// </summary>
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .ToList();

            if (failures.Count != 0)
                throw new ValidationException(failures);

            return await next();
        }
    }
}
