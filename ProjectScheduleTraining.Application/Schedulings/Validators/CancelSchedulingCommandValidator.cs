using FluentValidation;
using ProjectScheduleTraining.Application.Schedulings.Commands;

namespace ProjectScheduleTraining.Application.Schedulings.Validators
{
    /// <summary>
    /// Validator responsável por garantir que o identificador
    /// do agendamento a ser cancelado é válido.
    /// </summary>
    public class CancelSchedulingCommandValidator : AbstractValidator<CancelSchedulingCommand>
    {
        public CancelSchedulingCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador do agendamento é obrigatório.");
        }
    }
}
