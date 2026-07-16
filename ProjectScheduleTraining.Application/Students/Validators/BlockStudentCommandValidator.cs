using FluentValidation;
using ProjectScheduleTraining.Application.Students.Commands;

namespace ProjectScheduleTraining.Application.Students.Validators
{
    /// <summary>
    /// Validator responsável por garantir que o identificador
    /// do aluno a ser bloqueado é válido.
    /// </summary>
    public class BlockStudentCommandValidator : AbstractValidator<BlockStudentCommand>
    {
        public BlockStudentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador do aluno é obrigatório.");
        }
    }
}
