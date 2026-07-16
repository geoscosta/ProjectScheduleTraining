using FluentValidation;
using ProjectScheduleTraining.Application.Students.Commands;

namespace ProjectScheduleTraining.Application.Students.Validators
{
    /// <summary>
    /// Validator responsável por garantir que o identificador
    /// do aluno a ser inativado é válido.
    /// </summary>
    public class DeactivateStudentCommandValidator : AbstractValidator<DeactivateStudentCommand>
    {
        public DeactivateStudentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador do aluno é obrigatório.");
        }
    }
}
