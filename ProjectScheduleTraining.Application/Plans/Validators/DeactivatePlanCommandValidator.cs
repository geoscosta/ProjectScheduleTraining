using FluentValidation;
using ProjectScheduleTraining.Application.Plans.Commands;

namespace ProjectScheduleTraining.Application.Plans.Validators
{
    /// <summary>
    /// Validator responsável por garantir que o identificador
    /// do plano a ser desativado é válido.
    /// </summary>
    public class DeactivatePlanCommandValidator : AbstractValidator<DeactivatePlanCommand>
    {
        public DeactivatePlanCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador do plano é obrigatório.");
        }
    }
}
