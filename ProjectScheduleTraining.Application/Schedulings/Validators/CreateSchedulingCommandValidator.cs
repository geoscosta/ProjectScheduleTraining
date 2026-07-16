using FluentValidation;
using ProjectScheduleTraining.Application.Schedulings.Commands;

namespace ProjectScheduleTraining.Application.Schedulings.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para criação de um agendamento são válidos antes de chegar ao handler.
    /// </summary>
    public class CreateSchedulingCommandValidator : AbstractValidator<CreateSchedulingCommand>
    {
        public CreateSchedulingCommandValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Identificador do aluno é obrigatório.");

            RuleFor(x => x.ScheduleId)
                .NotEmpty().WithMessage("Identificador do horário é obrigatório.");
        }
    }
}
