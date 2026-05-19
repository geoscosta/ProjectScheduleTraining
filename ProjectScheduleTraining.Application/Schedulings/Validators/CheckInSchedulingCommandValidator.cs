using FluentValidation;
using ProjectScheduleTraining.Application.Schedulings.Commands;

namespace ProjectScheduleTraining.Application.Schedulings.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para registro de presença são válidos antes de chegar ao handler.
    /// </summary>
    public class CheckInSchedulingCommandValidator : AbstractValidator<CheckInSchedulingCommand>
    {
        public CheckInSchedulingCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador do agendamento é obrigatório.");

            RuleFor(x => x.TrainerNotes)
                .MaximumLength(1000).WithMessage("Observação não pode ter mais de 1000 caracteres.")
                .When(x => x.TrainerNotes is not null);
        }
    }
}
