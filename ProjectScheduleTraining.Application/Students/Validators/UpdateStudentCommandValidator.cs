using FluentValidation;
using ProjectScheduleTraining.Application.Students.Commands;

namespace ProjectScheduleTraining.Application.Students.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para atualização de um aluno são válidos antes de chegar ao handler.
    /// </summary>
    public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Identificador do aluno é obrigatório.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(150).WithMessage("Nome não pode ter mais de 150 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.")
                .MaximumLength(200).WithMessage("E-mail não pode ter mais de 200 caracteres.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefone é obrigatório.")
                .MaximumLength(20).WithMessage("Telefone não pode ter mais de 20 caracteres.");

            RuleFor(x => x.Address)
                .MaximumLength(300).WithMessage("Endereço não pode ter mais de 300 caracteres.")
                .When(x => x.Address is not null);

            RuleFor(x => x.EmergencyContact)
                .MaximumLength(200).WithMessage("Contato de emergência não pode ter mais de 200 caracteres.")
                .When(x => x.EmergencyContact is not null);
        }
    }
}
