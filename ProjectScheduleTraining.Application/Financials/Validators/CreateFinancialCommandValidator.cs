using FluentValidation;
using ProjectScheduleTraining.Application.Financials.Commands;

namespace ProjectScheduleTraining.Application.Financials.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para criação de uma cobrança financeira são válidos antes de chegar ao handler.
    /// </summary>
    public class CreateFinancialCommandValidator : AbstractValidator<CreateFinancialCommand>
    {
        public CreateFinancialCommandValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Identificador do aluno é obrigatório.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Valor deve ser maior que zero.");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("Data de vencimento é obrigatória.")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Data de vencimento não pode ser anterior à data atual.");

            RuleFor(x => x.Description)
                .MaximumLength(300).WithMessage("Descrição não pode ter mais de 300 caracteres.")
                .When(x => x.Description is not null);
        }
    }
}
