using FluentValidation;
using ProjectScheduleTraining.Application.Enrollments.Commands;

namespace ProjectScheduleTraining.Application.Enrollments.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para renovação de uma matrícula são válidos antes de chegar ao handler.
    /// </summary>
    public class RenewEnrollmentCommandValidator : AbstractValidator<RenewEnrollmentCommand>
    {
        public RenewEnrollmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador da matrícula é obrigatório.");

            RuleFor(x => x.AdditionalMonths)
                .GreaterThan(0).WithMessage("Quantidade de meses deve ser maior que zero.");
        }
    }
}
