using FluentValidation;
using ProjectScheduleTraining.Application.Enrollments.Commands;

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
                .NotEmpty().WithMessage("Identificador do aluno é obrigatório.");

            RuleFor(x => x.PlanId)
                .NotEmpty().WithMessage("Identificador do plano é obrigatório.");

            RuleFor(x => x.PaymentDueDay)
                .InclusiveBetween(1, 28)
                .WithMessage("Dia de vencimento deve ser entre 1 e 28.");
        }
    }
}
