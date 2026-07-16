using FluentValidation;
using ProjectScheduleTraining.Application.Schedules.Commands;

namespace ProjectScheduleTraining.Application.Schedules.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para bloqueio de um horário são válidos antes de chegar ao handler.
    /// </summary>
    public class BlockScheduleCommandValidator : AbstractValidator<BlockScheduleCommand>
    {
        public BlockScheduleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador do horário é obrigatório.");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Observação não pode ter mais de 500 caracteres.")
                .When(x => x.Notes is not null);
        }
    }
}
