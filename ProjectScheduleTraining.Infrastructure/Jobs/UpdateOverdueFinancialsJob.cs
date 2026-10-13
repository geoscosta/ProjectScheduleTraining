using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Infrastructure.Jobs;

/// <summary>
/// Job responsável por atualizar o status de cobranças pendentes
/// vencidas para Overdue diariamente.
/// </summary>
public class UpdateOverdueFinancialsJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<UpdateOverdueFinancialsJob> _logger;

    private static readonly TimeSpan CheckInterval = TimeSpan.FromHours(6);

    public UpdateOverdueFinancialsJob(
        IServiceScopeFactory scopeFactory,
        ILogger<UpdateOverdueFinancialsJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("UpdateOverdueFinancialsJob iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UpdateOverdueAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar UpdateOverdueFinancialsJob.");
            }

            await Task.Delay(CheckInterval, stoppingToken);
        }
    }

    private async Task UpdateOverdueAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        await unitOfWork.Financials.UpdateOverdueStatusAsync(cancellationToken);

        _logger.LogInformation("Status de cobranças vencidas atualizado.");
    }
}