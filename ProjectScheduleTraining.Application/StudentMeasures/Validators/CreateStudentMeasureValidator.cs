using FluentValidation;
using ProjectScheduleTraining.Application.StudentMeasures.Commands;

namespace ProjectScheduleTraining.Application.StudentMeasures.Validators
{
    /// <summary>
    /// Validator responsável por validar o comando de registro de medidas corporais.
    /// </summary>
    public class CreateStudentMeasureValidator
        : AbstractValidator<CreateStudentMeasureCommand>
    {
        public CreateStudentMeasureValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty()
                .WithMessage("Aluno é obrigatório.");

            RuleFor(x => x.MeasureDate)
                .NotEmpty()
                .WithMessage("Data da avaliação é obrigatória.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Data da avaliação não pode ser futura.");

            RuleFor(x => x.Weight)
                .GreaterThan(0)
                .WithMessage("Peso deve ser maior que zero.")
                .LessThanOrEqualTo(500)
                .WithMessage("Peso inválido.");

            RuleFor(x => x.Height)
                .GreaterThan(0)
                .WithMessage("Altura deve ser maior que zero.")
                .LessThanOrEqualTo(300)
                .WithMessage("Altura inválida.");

            /// Circunferências são opcionais mas devem ser válidas se informadas.
            When(x => x.ChestCircumference.HasValue, () =>
            {
                RuleFor(x => x.ChestCircumference!.Value)
                    .GreaterThan(0).WithMessage("Circunferência do peito inválida.")
                    .LessThanOrEqualTo(300).WithMessage("Circunferência do peito inválida.");
            });

            When(x => x.WaistCircumference.HasValue, () =>
            {
                RuleFor(x => x.WaistCircumference!.Value)
                    .GreaterThan(0).WithMessage("Circunferência da cintura inválida.")
                    .LessThanOrEqualTo(300).WithMessage("Circunferência da cintura inválida.");
            });

            When(x => x.HipCircumference.HasValue, () =>
            {
                RuleFor(x => x.HipCircumference!.Value)
                    .GreaterThan(0).WithMessage("Circunferência do quadril inválida.")
                    .LessThanOrEqualTo(300).WithMessage("Circunferência do quadril inválida.");
            });

            When(x => x.ArmCircumference.HasValue, () =>
            {
                RuleFor(x => x.ArmCircumference!.Value)
                    .GreaterThan(0).WithMessage("Circunferência do braço inválida.")
                    .LessThanOrEqualTo(100).WithMessage("Circunferência do braço inválida.");
            });

            When(x => x.ThighCircumference.HasValue, () =>
            {
                RuleFor(x => x.ThighCircumference!.Value)
                    .GreaterThan(0).WithMessage("Circunferência da coxa inválida.")
                    .LessThanOrEqualTo(200).WithMessage("Circunferência da coxa inválida.");
            });

            When(x => x.CalfCircumference.HasValue, () =>
            {
                RuleFor(x => x.CalfCircumference!.Value)
                    .GreaterThan(0).WithMessage("Circunferência da panturrilha inválida.")
                    .LessThanOrEqualTo(100).WithMessage("Circunferência da panturrilha inválida.");
            });

            When(x => x.BodyFatPercentage.HasValue, () =>
            {
                RuleFor(x => x.BodyFatPercentage!.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Percentual de gordura inválido.")
                    .LessThanOrEqualTo(100).WithMessage("Percentual de gordura inválido.");
            });

            When(x => x.LeanMassPercentage.HasValue, () =>
            {
                RuleFor(x => x.LeanMassPercentage!.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Percentual de massa magra inválido.")
                    .LessThanOrEqualTo(100).WithMessage("Percentual de massa magra inválido.");
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
