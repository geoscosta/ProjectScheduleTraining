using FluentValidation;
using ProjectScheduleTraining.Application.ParQAssessments.Commands;

namespace ProjectScheduleTraining.Application.ParQAssessments.Validators
{
    /// <summary>
    /// Validator responsável por validar o comando de registro da avaliação PAR-Q.
    /// </summary>
    public class CreateParQAssessmentValidator
        : AbstractValidator<CreateParQAssessmentCommand>
    {
        public CreateParQAssessmentValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty()
                .WithMessage("Aluno é obrigatório.");

            /// As 7 questões do PAR-Q são obrigatórias.
            /// Cada questão é um boolean — não há validação de valor,
            /// apenas de presença no payload.
            RuleFor(x => x.Question1).NotNull()
                .WithMessage("Questão 1 é obrigatória.");
            RuleFor(x => x.Question2).NotNull()
                .WithMessage("Questão 2 é obrigatória.");
            RuleFor(x => x.Question3).NotNull()
                .WithMessage("Questão 3 é obrigatória.");
            RuleFor(x => x.Question4).NotNull()
                .WithMessage("Questão 4 é obrigatória.");
            RuleFor(x => x.Question5).NotNull()
                .WithMessage("Questão 5 é obrigatória.");
            RuleFor(x => x.Question6).NotNull()
                .WithMessage("Questão 6 é obrigatória.");
            RuleFor(x => x.Question7).NotNull()
                .WithMessage("Questão 7 é obrigatória.");

            When(x => x.Notes is not null, () =>
            {
                RuleFor(x => x.Notes)
                    .MaximumLength(500)
                    .WithMessage("Observações não podem ter mais de 500 caracteres.");
            });
        }
    }
}
