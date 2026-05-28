using FluentValidation;
using ProjectScheduleTraining.Application.ScheduleLocks.Commands;

namespace ProjectScheduleTraining.Application.ScheduleLocks.Validators
{
    /// <summary>
    /// Validator responsável por validar o comando de solicitação de trancamento de agenda.
    /// </summary>
    public class CreateScheduleLockValidator
        : AbstractValidator<CreateScheduleLockCommand>
    {
        public CreateScheduleLockValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty()
                .WithMessage("Aluno é obrigatório.");

            RuleFor(x => x.EnrollmentId)
                .NotEmpty()
                .WithMessage("Matrícula é obrigatória.");

            RuleFor(x => x.LockStartDate)
                .NotEmpty()
                .WithMessage("Data de início do trancamento é obrigatória.")
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .WithMessage("Data de início não pode ser no passado.");

            RuleFor(x => x.LockEndDate)
                .NotEmpty()
                .WithMessage("Data de fim do trancamento é obrigatória.")
                .GreaterThan(x => x.LockStartDate)
                .WithMessage("Data de fim deve ser maior que a data de início.");

            /// Valida que o período de trancamento não ultrapassa 90 dias.
            RuleFor(x => x)
                .Must(x => (x.LockEndDate - x.LockStartDate).TotalDays <= 90)
                .WithMessage("O período de trancamento não pode ultrapassar 90 dias.")
                .OverridePropertyName("LockEndDate");

            RuleFor(x => x.Justification)
                .IsInEnum()
                .WithMessage("Justificativa inválida.");

            When(x => x.DocumentUrl is not null, () =>
            {
                RuleFor(x => x.DocumentUrl)
                    .MaximumLength(500)
                    .WithMessage("URL do documento não pode ter mais de 500 caracteres.");
            });

            When(x => x.Notes is not null, () =>
            {
                RuleFor(x => x.Notes)
                    .MaximumLength(500)
                    .WithMessage("Observações não podem ter mais de 500 caracteres.");
            });
        }
    }
}
