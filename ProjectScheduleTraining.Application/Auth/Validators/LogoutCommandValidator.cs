using FluentValidation;
using ProjectScheduleTraining.Application.Auth.Commands;

namespace ProjectScheduleTraining.Application.Auth.Validators
{
    /// <summary>
    /// Validator responsável por garantir que o identificador
    /// do usuário para logout é válido.
    /// </summary>
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Identificador do usuário é obrigatório.");
        }
    }
}
