using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Infrastructure.Jobs;

/// <summary>
/// Job responsável por bloquear automaticamente os agendamentos
/// durante o recesso de fim de ano (2 semanas: Natal e Ano Novo).
/// Pagamento é mantido normalmente. Sem reposição de aulas.
/// </summary>
public class YearEndRecessJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<YearEndRecessJob> _logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;

            /// Executa apenas em dezembro para criar o bloqueio do recesso.
            if (now.Month == 12 && now.Day == 1)
            {
                await BlockRecessSchedulesAsync(now.Year, stoppingToken);
            }

            /// Verifica diariamente às 01:00.
            var nextRun = now.Date.AddDays(1).AddHours(1);
            var delay = nextRun - now;
            await Task.Delay(delay, stoppingToken);
        }
    }

    /// <summary>
    /// Bloqueia horários durante o recesso de fim de ano.
    /// Período: 23 de dezembro ao 5 de janeiro do ano seguinte.
    /// </summary>
    private async Task BlockRecessSchedulesAsync(int year, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var recessStart = new DateTime(year, 12, 23);
        var recessEnd = new DateTime(year + 1, 1, 5);

        var schedules = await unitOfWork.Schedules
            .GetByPeriodAsync(recessStart, recessEnd, cancellationToken);

        var blockedCount = 0;

        foreach (var schedule in schedules.Where(s => s.Status == ScheduleStatus.Available))
        {
            schedule.Status = ScheduleStatus.Blocked;
            schedule.Notes = "Recesso de fim de ano — sem reposição de aulas.";
            unitOfWork.Schedules.Update(schedule);
            blockedCount++;
        }

        if (blockedCount > 0)
        {
            await unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation(
                "{Count} horário(s) bloqueado(s) para o recesso de fim de ano {Year}.",
                blockedCount, year);
        }
    }
}