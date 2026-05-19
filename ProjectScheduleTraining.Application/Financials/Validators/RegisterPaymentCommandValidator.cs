using FluentValidation;
using ProjectScheduleTraining.Application.Financials.Commands;

namespace ProjectScheduleTraining.Application.Financials.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para registro de pagamento são válidos antes de chegar ao handler.
    /// </summary>
    public class RegisterPaymentCommandValidator : AbstractValidator<RegisterPaymentCommand>
    {
        public RegisterPaymentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador da cobrança é obrigatório.");

            RuleFor(x => x.PaymentProof)
                .MaximumLength(500).WithMessage("Comprovante não pode ter mais de 500 caracteres.")
                .When(x => x.PaymentProof is not null);
        }
    }
}
