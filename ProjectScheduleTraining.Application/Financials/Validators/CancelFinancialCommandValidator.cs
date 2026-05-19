using FluentValidation;
using ProjectScheduleTraining.Application.Financials.Commands;

namespace ProjectScheduleTraining.Application.Financials.Validators
{
    /// <summary>
    /// Validator responsável por garantir que o identificador
    /// da cobrança a ser cancelada é válido.
    /// </summary>
    public class CancelFinancialCommandValidator : AbstractValidator<CancelFinancialCommand>
    {
        public CancelFinancialCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador da cobrança é obrigatório.");
        }
    }
}
