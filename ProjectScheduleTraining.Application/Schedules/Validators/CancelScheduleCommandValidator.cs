using FluentValidation;
using ProjectScheduleTraining.Application.Schedules.Commands;

namespace ProjectScheduleTraining.Application.Schedules.Validators
{
    /// <summary>
    /// Validator responsável por garantir que o identificador
    /// do horário a ser cancelado é válido.
    /// </summary>
    public class CancelScheduleCommandValidator : AbstractValidator<CancelScheduleCommand>
    {
        public CancelScheduleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador do horário é obrigatório.");
        }
    }
}
