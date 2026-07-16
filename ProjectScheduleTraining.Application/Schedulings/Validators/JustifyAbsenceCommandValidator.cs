using FluentValidation;
using ProjectScheduleTraining.Application.Schedulings.Commands;

namespace ProjectScheduleTraining.Application.Schedulings.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para registro de falta justificada são válidos antes de chegar ao handler.
    /// </summary>
    public class JustifyAbsenceCommandValidator : AbstractValidator<JustifyAbsenceCommand>
    {
        public JustifyAbsenceCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador do agendamento é obrigatório.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Motivo da falta justificada é obrigatório.")
                .MaximumLength(500).WithMessage("Motivo não pode ter mais de 500 caracteres.");
        }
    }
}
