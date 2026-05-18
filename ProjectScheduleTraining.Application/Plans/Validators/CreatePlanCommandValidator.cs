using FluentValidation;
using ProjectScheduleTraining.Application.Plans.Commands;

namespace ProjectScheduleTraining.Application.Plans.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para criação de um plano são válidos antes de chegar ao handler.
    /// </summary>
    public class CreatePlanCommandValidator : AbstractValidator<CreatePlanCommand>
    {
        public CreatePlanCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome do plano é obrigatório.")
                .MaximumLength(100).WithMessage("Nome não pode ter mais de 100 caracteres.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Tipo de plano inválido.");

            RuleFor(x => x.WeeklyFrequency)
                .IsInEnum().WithMessage("Frequência semanal inválida.");

            RuleFor(x => x.DurationMonths)
                .GreaterThan(0).WithMessage("Duração deve ser maior que zero.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Valor do plano deve ser maior que zero.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Descrição não pode ter mais de 500 caracteres.")
                .When(x => x.Description is not null);
        }
    }
}
