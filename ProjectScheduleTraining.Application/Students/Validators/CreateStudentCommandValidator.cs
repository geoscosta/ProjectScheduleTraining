using FluentValidation;
using ProjectScheduleTraining.Application.Students.Commands;
using ProjectScheduleTraining.CrossCutting.Helpers;

namespace ProjectScheduleTraining.Application.Students.Validators;

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

        /// Endereço estruturado — campos opcionais mas com tamanho máximo.
        When(x => x.Street is not null, () =>
        {
            RuleFor(x => x.Street)
                .MaximumLength(200).WithMessage("Rua não pode ter mais de 200 caracteres.");
        });

        When(x => x.AddressNumber is not null, () =>
        {
            RuleFor(x => x.AddressNumber)
                .MaximumLength(20).WithMessage("Número não pode ter mais de 20 caracteres.");
        });

        When(x => x.Complement is not null, () =>
        {
            RuleFor(x => x.Complement)
                .MaximumLength(100).WithMessage("Complemento não pode ter mais de 100 caracteres.");
        });

        When(x => x.District is not null, () =>
        {
            RuleFor(x => x.District)
                .MaximumLength(100).WithMessage("Bairro não pode ter mais de 100 caracteres.");
        });

        When(x => x.City is not null, () =>
        {
            RuleFor(x => x.City)
                .MaximumLength(100).WithMessage("Cidade não pode ter mais de 100 caracteres.");
        });

        When(x => x.State is not null, () =>
        {
            RuleFor(x => x.State)
                .Length(2).WithMessage("Estado deve ter 2 caracteres (ex: CE, SP).");
        });

        When(x => x.ZipCode is not null, () =>
        {
            RuleFor(x => x.ZipCode)
                .MaximumLength(9).WithMessage("CEP não pode ter mais de 9 caracteres.");
        });

        /// Responsável — obrigatório para menores de 18 anos.
        When(x => x.BirthDate != default &&
                  DateTime.UtcNow.Year - x.BirthDate.Year < 18, () =>
                  {
                      RuleFor(x => x.GuardianName)
                          .NotEmpty()
                          .WithMessage("Nome do responsável é obrigatório para menores de 18 anos.");

                      RuleFor(x => x.GuardianCpf)
                          .NotEmpty()
                          .WithMessage("CPF do responsável é obrigatório para menores de 18 anos.")
                          .Must(cpf => cpf is null || CpfHelper.IsValid(cpf))
                          .WithMessage("CPF do responsável inválido.");
                  });

        When(x => x.GuardianName is not null, () =>
        {
            RuleFor(x => x.GuardianName)
                .MaximumLength(150).WithMessage("Nome do responsável não pode ter mais de 150 caracteres.");
        });

        When(x => x.EmergencyContact is not null, () =>
        {
            RuleFor(x => x.EmergencyContact)
                .MaximumLength(200).WithMessage("Contato de emergência não pode ter mais de 200 caracteres.");
        });

        When(x => x.IdentityDocument is not null, () =>
        {
            RuleFor(x => x.IdentityDocument)
                .MaximumLength(20).WithMessage("RG não pode ter mais de 20 caracteres.");
        });

        When(x => x.Profession is not null, () =>
        {
            RuleFor(x => x.Profession)
                .MaximumLength(100).WithMessage("Profissão não pode ter mais de 100 caracteres.");
        });
    }
}