using FluentValidation;
using ProjectScheduleTraining.Application.Auth.Commands;

namespace ProjectScheduleTraining.Application.Auth.Validators
{
    /// <summary>
    /// Validator responsável por garantir que o refresh token
    /// informado é válido antes de chegar ao handler.
    /// </summary>
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token é obrigatório.");
        }
    }
}
