using FluentValidation;
using ProjectScheduleTraining.Application.Auth.Commands;

namespace ProjectScheduleTraining.Application.Auth.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para login são válidos antes de chegar ao handler.
    /// </summary>
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória.");
        }
    }
}
