using FluentValidation;
using ProjectScheduleTraining.Application.Auth.Commands;

namespace ProjectScheduleTraining.Application.Auth.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para criação de um usuário são válidos antes de chegar ao handler.
    /// </summary>
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(150).WithMessage("Nome não pode ter mais de 150 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.")
                .MaximumLength(200).WithMessage("E-mail não pode ter mais de 200 caracteres.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres.")
                .Matches("[A-Z]").WithMessage("Senha deve conter pelo menos uma letra maiúscula.")
                .Matches("[a-z]").WithMessage("Senha deve conter pelo menos uma letra minúscula.")
                .Matches("[0-9]").WithMessage("Senha deve conter pelo menos um número.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Senha deve conter pelo menos um caractere especial.");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Perfil de usuário inválido.");
        }
    }
}
