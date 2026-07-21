using FluentValidation;
using ProjectScheduleTraining.Application.Enrollments.Commands;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Enrollments.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para criação de uma matrícula são válidos antes de chegar ao handler.
    /// </summary>
    public class CreateEnrollmentCommandValidator : AbstractValidator<CreateEnrollmentCommand>
    {
        public CreateEnrollmentCommandValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Aluno é obrigatório.");

            RuleFor(x => x.PlanId)
                .NotEmpty().WithMessage("Plano é obrigatório.");

            RuleFor(x => x.PaymentDueDay)
                .Must(day => Enum.IsDefined(typeof(PaymentDueDay), day))
                .WithMessage("Dia de vencimento deve ser 5, 10, 15 ou 20.");

            RuleFor(x => x.PaymentMethod)
            .IsInEnum()
            .WithMessage("Método de pagamento inválido.");
        }
    }
}
