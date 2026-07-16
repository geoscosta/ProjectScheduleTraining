using FluentValidation;
using ProjectScheduleTraining.Application.Enrollments.Commands;

namespace ProjectScheduleTraining.Application.Enrollments.Validators
{
    /// <summary>
    /// Validator responsável por garantir que o identificador
    /// da matrícula a ser cancelada é válido.
    /// </summary>
    public class CancelEnrollmentCommandValidator : AbstractValidator<CancelEnrollmentCommand>
    {
        public CancelEnrollmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador da matrícula é obrigatório.");
        }
    }
}
