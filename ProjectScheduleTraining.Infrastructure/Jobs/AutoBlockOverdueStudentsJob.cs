using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Infrastructure.Jobs;

/// <summary>
/// Job responsável por bloquear automaticamente alunos
/// com cobranças vencidas há mais de 2 dias conforme contrato.
/// Executa diariamente às 08:00.
/// </summary>
public class AutoBlockOverdueStudentsJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AutoBlockOverdueStudentsJob> _logger;

    /// Dias de atraso antes de bloquear conforme contrato (Cláusula 32).
    private const int DaysOverdueForBlock = 2;

    /// Intervalo de execução do job — verifica a cada 6 horas.
    private static readonly TimeSpan CheckInterval = TimeSpan.FromHours(6);

    public AutoBlockOverdueStudentsJob(
        IServiceScopeFactory scopeFactory,
        ILogger<AutoBlockOverdueStudentsJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AutoBlockOverdueStudentsJob iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await BlockOverdueStudentsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar AutoBlockOverdueStudentsJob.");
            }

            await Task.Delay(CheckInterval, stoppingToken);
        }
    }

    /// <summary>
    /// Busca alunos com cobranças vencidas há mais de 2 dias
    /// e bloqueia automaticamente os que ainda estão ativos.
    /// </summary>
    private async Task BlockOverdueStudentsAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var overdueStudentIds = await unitOfWork.Financials
            .GetStudentIdsWithOverdueFinancialsAsync(
                DaysOverdueForBlock,
                cancellationToken);

        var blockedCount = 0;

        foreach (var studentId in overdueStudentIds)
        {
            var student = await unitOfWork.Students
                .GetByIdAsync(studentId, cancellationToken);

            if (student is null || student.Status != StudentStatus.Active)
                continue;

            student.Status = StudentStatus.Blocked;
            unitOfWork.Students.Update(student);
            blockedCount++;

            _logger.LogWarning(
                "Aluno {StudentId} ({StudentName}) bloqueado automaticamente por inadimplência.",
                student.Id,
                student.Name);
        }

        if (blockedCount > 0)
        {
            await unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation(
                "{Count} aluno(s) bloqueado(s) automaticamente por inadimplência.",
                blockedCount);
        }
    }
}