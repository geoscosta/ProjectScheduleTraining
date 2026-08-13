using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Infrastructure.Jobs;

/// <summary>
/// Job responsável por cancelar automaticamente matrículas
/// de alunos com cobranças vencidas há mais de 2 dias
/// e liberar a vaga conforme novo contrato (Cláusula 3).
/// </summary>
public class AutoBlockOverdueStudentsJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AutoBlockOverdueStudentsJob> _logger;

    /// Dias de atraso antes de cancelar conforme novo contrato.
    private const int DaysOverdueForCancel = 2;

    /// Intervalo de execução — verifica a cada 6 horas.
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
                await CancelOverdueEnrollmentsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar AutoBlockOverdueStudentsJob.");
            }

            await Task.Delay(CheckInterval, stoppingToken);
        }
    }

    /// <summary>
    /// Cancela automaticamente matrículas com cobranças vencidas
    /// há mais de 2 dias e libera a vaga para outros alunos.
    /// Conforme novo contrato: atraso ou falta de comunicação
    /// resulta em cancelamento da matrícula e disponibilização da vaga.
    /// </summary>
    private async Task CancelOverdueEnrollmentsAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var overdueStudentIds = await unitOfWork.Financials
            .GetStudentIdsWithOverdueFinancialsAsync(
                DaysOverdueForCancel,
                cancellationToken);

        var cancelledCount = 0;

        foreach (var studentId in overdueStudentIds)
        {
            /// Cancela a matrícula ativa do aluno inadimplente.
            var enrollment = await unitOfWork.Enrollments
                .GetByStudentIdAsync(studentId, cancellationToken);

            if (enrollment is null || !enrollment.IsActive)
                continue;

            enrollment.IsActive = false;
            unitOfWork.Enrollments.Update(enrollment);
            cancelledCount++;

            _logger.LogWarning(
                "Matrícula {EnrollmentId} do aluno {StudentId} cancelada automaticamente por inadimplência.",
                enrollment.Id,
                studentId);
        }

        if (cancelledCount > 0)
        {
            await unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation(
                "{Count} matrícula(s) cancelada(s) automaticamente por inadimplência.",
                cancelledCount);
        }
    }
}