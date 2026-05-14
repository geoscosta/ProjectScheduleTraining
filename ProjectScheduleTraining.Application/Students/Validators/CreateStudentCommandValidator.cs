using FluentValidation;
using ProjectScheduleTraining.Application.Students.Commands;
using ProjectScheduleTraining.CrossCutting.Helpers;

namespace ProjectScheduleTraining.Application.Students.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para criação de um aluno são válidos antes de chegar ao handler.
    /// </summary>
    public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(150).WithMessage("Nome não pode ter mais de 150 caracteres.");

            RuleFor(x => x.Cpf)
                .NotEmpty().WithMessage("CPF é obrigatório.")
                .Must(CpfHelper.IsValid).WithMessage("CPF inválido.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.")
                .MaximumLength(200).WithMessage("E-mail não pode ter mais de 200 caracteres.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefone é obrigatório.")
                .MaximumLength(20).WithMessage("Telefone não pode ter mais de 20 caracteres.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Data de nascimento é obrigatória.")
                .LessThan(DateTime.Today).WithMessage("Data de nascimento inválida.")
                .GreaterThan(DateTime.Today.AddYears(-120)).WithMessage("Data de nascimento inválida.");

            RuleFor(x => x.Address)
                .MaximumLength(300).WithMessage("Endereço não pode ter mais de 300 caracteres.")
                .When(x => x.Address is not null);

            RuleFor(x => x.EmergencyContact)
                .MaximumLength(200).WithMessage("Contato de emergência não pode ter mais de 200 caracteres.")
                .When(x => x.EmergencyContact is not null);
        }
    }
}
