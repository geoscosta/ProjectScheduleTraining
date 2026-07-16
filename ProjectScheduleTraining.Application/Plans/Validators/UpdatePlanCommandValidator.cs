using FluentValidation;
using ProjectScheduleTraining.Application.Plans.Commands;

namespace ProjectScheduleTraining.Application.Plans.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para atualização de um plano são válidos antes de chegar ao handler.
    /// </summary>
    public class UpdatePlanCommandValidator : AbstractValidator<UpdatePlanCommand>
    {
        public UpdatePlanCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador do plano é obrigatório.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome do plano é obrigatório.")
                .MaximumLength(100).WithMessage("Nome não pode ter mais de 100 caracteres.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Valor do plano deve ser maior que zero.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Descrição não pode ter mais de 500 caracteres.")
                .When(x => x.Description is not null);
        }
    }
}
