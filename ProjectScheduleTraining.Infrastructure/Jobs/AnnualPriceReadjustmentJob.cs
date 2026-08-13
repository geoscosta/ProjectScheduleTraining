using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Infrastructure.Jobs;

/// <summary>
/// Job responsável por aplicar o reajuste anual de preços
/// baseado no IPCA-M ou índice configurável.
/// Executa em 1º de janeiro de cada ano.
/// Impacta apenas planos na renovação — contratos vigentes não são alterados.
/// </summary>
public class AnnualPriceReadjustmentJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AnnualPriceReadjustmentJob> _logger;
    private readonly IConfiguration _configuration;

    public AnnualPriceReadjustmentJob(
        IServiceScopeFactory scopeFactory,
        ILogger<AnnualPriceReadjustmentJob> logger,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;

            /// Executa em 1º de janeiro às 02:00.
            if (now.Month == 1 && now.Day == 1 && now.Hour == 2)
            {
                await ApplyReadjustmentAsync(stoppingToken);
            }

            var nextRun = now.Date.AddDays(1).AddHours(2);
            var delay = nextRun - now;
            await Task.Delay(delay, stoppingToken);
        }
    }

    /// <summary>
    /// Aplica o percentual de reajuste configurado em appsettings
    /// sobre todos os planos ativos.
    /// </summary>
    private async Task ApplyReadjustmentAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        /// Lê o percentual de reajuste do appsettings (IPCA-M ou configurável).
        var readjustmentPercent = _configuration
            .GetValue<decimal>("Studio:AnnualReadjustmentPercent", 4.5m);

        var plans = await unitOfWork.Plans.GetAllAsync(cancellationToken);
        var activePlans = plans.Where(p => p.IsActive).ToList();

        foreach (var plan in activePlans)
        {
            var oldPrice = plan.Price;
            plan.Price = Math.Round(plan.Price * (1 + readjustmentPercent / 100), 2);
            unitOfWork.Plans.Update(plan);

            _logger.LogInformation(
                "Plano {PlanName}: R$ {OldPrice} → R$ {NewPrice} ({Percent}% IPCA-M)",
                plan.Name, oldPrice, plan.Price, readjustmentPercent);
        }

        await unitOfWork.CommitAsync(cancellationToken);
        _logger.LogInformation(
            "Reajuste anual de {Percent}% aplicado em {Count} plano(s).",
            readjustmentPercent, activePlans.Count);
    }
}