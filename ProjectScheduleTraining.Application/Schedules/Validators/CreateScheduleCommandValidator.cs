using FluentValidation;
using ProjectScheduleTraining.Application.Schedules.Commands;

namespace ProjectScheduleTraining.Application.Schedules.Validators
{
    /// <summary>
    /// Validator responsável por garantir que os dados informados
    /// para criação de um horário são válidos antes de chegar ao handler.
    /// </summary>
    public class CreateScheduleCommandValidator : AbstractValidator<CreateScheduleCommand>
    {
        public CreateScheduleCommandValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Data é obrigatória.")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Data não pode ser anterior à data atual.")
                .Must(BeAWeekday)
                .WithMessage("Aulas só podem ser criadas de segunda a sexta.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Hora de início é obrigatória.")
                .GreaterThanOrEqualTo(TimeSpan.FromHours(6))
                .WithMessage("Horário de início não pode ser antes das 06h.")
                .LessThanOrEqualTo(TimeSpan.FromHours(19))
                .WithMessage("Horário de início não pode ser após as 19h.");
        }

        /// <summary>
        /// Verifica se a data informada é um dia útil (segunda a sexta).
        /// </summary>
        private static bool BeAWeekday(DateTime date)
            => date.DayOfWeek != DayOfWeek.Saturday &&
               date.DayOfWeek != DayOfWeek.Sunday;
    }
}
