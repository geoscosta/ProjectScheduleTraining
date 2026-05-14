using FluentValidation;
using ProjectScheduleTraining.Application.Students.Commands;

namespace ProjectScheduleTraining.Application.Students.Validators
{
    /// <summary>
    /// Validator responsável por garantir que o identificador
    /// do aluno a ser desbloqueado é válido.
    /// </summary>
    public class UnblockStudentCommandValidator : AbstractValidator<UnblockStudentCommand>
    {
        public UnblockStudentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador do aluno é obrigatório.");
        }
    }
}
