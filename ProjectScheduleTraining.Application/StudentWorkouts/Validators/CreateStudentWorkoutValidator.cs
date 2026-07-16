using FluentValidation;
using ProjectScheduleTraining.Application.StudentWorkouts.Commands;

namespace ProjectScheduleTraining.Application.StudentWorkouts.Validators
{
    /// <summary>
    /// Validator responsável por validar o comando de criação de treino personalizado.
    /// </summary>
    public class CreateStudentWorkoutValidator
        : AbstractValidator<CreateStudentWorkoutCommand>
    {
        public CreateStudentWorkoutValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty()
                .WithMessage("Aluno é obrigatório.");

            RuleFor(x => x.TrainerId)
                .NotEmpty()
                .WithMessage("Professor é obrigatório.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Nome do treino é obrigatório.")
                .MaximumLength(100)
                .WithMessage("Nome do treino não pode ter mais de 100 caracteres.");

            When(x => x.Description is not null, () =>
            {
                RuleFor(x => x.Description)
                    .MaximumLength(500)
                    .WithMessage("Descrição não pode ter mais de 500 caracteres.");
            });

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Data de início é obrigatória.");

            When(x => x.EndDate.HasValue, () =>
            {
                RuleFor(x => x.EndDate!.Value)
                    .GreaterThan(x => x.StartDate)
                    .WithMessage("Data de fim deve ser maior que a data de início.");
            });

            RuleFor(x => x.Exercises)
                .NotNull()
                .WithMessage("Lista de exercícios é obrigatória.")
                .NotEmpty()
                .WithMessage("O treino deve ter pelo menos 1 exercício.");

            RuleForEach(x => x.Exercises).SetValidator(
                new CreateWorkoutExerciseValidator());
        }
    }

    /// <summary>
    /// Validator responsável por validar cada exercício do treino.
    /// </summary>
    public class CreateWorkoutExerciseValidator
        : AbstractValidator<CreateWorkoutExerciseDto>
    {
        public CreateWorkoutExerciseValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Nome do exercício é obrigatório.")
                .MaximumLength(100)
                .WithMessage("Nome do exercício não pode ter mais de 100 caracteres.");

            RuleFor(x => x.MuscleGroup)
                .NotEmpty()
                .WithMessage("Grupo muscular é obrigatório.")
                .MaximumLength(100)
                .WithMessage("Grupo muscular não pode ter mais de 100 caracteres.");

            RuleFor(x => x.Sets)
                .GreaterThan(0)
                .WithMessage("Número de séries deve ser maior que zero.")
                .LessThanOrEqualTo(20)
                .WithMessage("Número de séries não pode ser maior que 20.");

            RuleFor(x => x.Repetitions)
                .NotEmpty()
                .WithMessage("Repetições são obrigatórias.")
                .MaximumLength(20)
                .WithMessage("Repetições não podem ter mais de 20 caracteres.");

            When(x => x.Load.HasValue, () =>
            {
                RuleFor(x => x.Load!.Value)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Carga não pode ser negativa.")
                    .LessThanOrEqualTo(500)
                    .WithMessage("Carga inválida.");
            });

            When(x => x.RestSeconds.HasValue, () =>
            {
                RuleFor(x => x.RestSeconds!.Value)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Tempo de descanso não pode ser negativo.")
                    .LessThanOrEqualTo(600)
                    .WithMessage("Tempo de descanso não pode ser maior que 10 minutos.");
            });

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Ordem do exercício inválida.");

            When(x => x.VideoUrl is not null, () =>
            {
                RuleFor(x => x.VideoUrl)
                    .MaximumLength(500)
                    .WithMessage("URL do vídeo não pode ter mais de 500 caracteres.");
            });
        }
    }
}
