using MediatR;
using Microsoft.Extensions.Logging;
using ProjectScheduleTraining.Application.Enrollments.Commands;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Enrollments.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de cancelamento de uma matrícula.
    /// Para planos fidelidade o aluno pode:
    /// 1. Pagar multa de 20% sobre o saldo remanescente
    /// 2. Indicar um substituto para ocupar a vaga (sem multa)
    /// Valida a existência e o status da matrícula antes de cancelar.
    /// </summary>
    public class CancelEnrollmentHandler : IRequestHandler<CancelEnrollmentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CancelEnrollmentHandler> _logger;

        public CancelEnrollmentHandler(IUnitOfWork unitOfWork, ILogger<CancelEnrollmentHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// Tipos de plano com fidelidade que possuem regras especiais de cancelamento.
        private static readonly PlanType[] LoyaltyPlanTypes =
        [
            PlanType.Quarterly,
            PlanType.SemiAnnual,
            PlanType.Annual
        ];

        /// <summary>
        /// Processa o comando de cancelamento da matrícula.
        /// Lança exceção caso a matrícula não seja encontrada ou já esteja cancelada.
        /// </summary>
        public async Task Handle(
            CancelEnrollmentCommand request,
            CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.Enrollments
                .GetByIdAsync(request.Id, cancellationToken);

            if (enrollment is null)
                throw new DomainException(
                    "Matrícula não encontrada.",
                    "ENROLLMENT_NOT_FOUND");

            if (!enrollment.IsActive)
                throw new DomainException(
                    "Matrícula já está cancelada.",
                    "ENROLLMENT_ALREADY_CANCELLED");

            var plan = await _unitOfWork.Plans
                .GetByIdAsync(enrollment.PlanId, cancellationToken);

            /// Verifica se é plano fidelidade — possui regras especiais de cancelamento.
            var isLoyaltyPlan = plan is not null && LoyaltyPlanTypes.Contains(plan.Type);

            if (isLoyaltyPlan)
            {
                if (request.CancellationOption is null)
                    throw new DomainException(
                        "Para planos fidelidade, informe a opção de cancelamento: " +
                        "pagar multa de 20% ou indicar um substituto para a vaga.",
                        "CANCELLATION_OPTION_REQUIRED");

                if (request.CancellationOption == CancellationOption.PayPenalty)
                {
                    /// Calcula multa de 20% sobre o saldo remanescente do contrato.
                    var remainingDays = (enrollment.ExpirationDate - DateTime.UtcNow).TotalDays;
                    var totalDays = (enrollment.ExpirationDate - enrollment.StartDate).TotalDays;
                    var remainingRatio = Math.Max(0, remainingDays / totalDays);
                    var penalty = Math.Round(enrollment.FinalPrice * 0.20m * (decimal)remainingRatio, 2);

                    enrollment.CancellationOption = CancellationOption.PayPenalty;
                    enrollment.CancellationPenaltyAmount = penalty;

                    _logger.LogInformation(
                        "Cancelamento com multa de R$ {Penalty} para matrícula {EnrollmentId}.",
                        penalty, enrollment.Id);
                }
                else if (request.CancellationOption == CancellationOption.SubstituteStudent)
                {
                    /// Valida se o substituto foi informado.
                    if (request.SubstituteStudentId is null)
                        throw new DomainException(
                            "Informe o aluno substituto para transferência da vaga.",
                            "SUBSTITUTE_STUDENT_REQUIRED");

                    var substitute = await _unitOfWork.Students
                        .GetByIdAsync(request.SubstituteStudentId.Value, cancellationToken);

                    if (substitute is null)
                        throw new DomainException(
                            "Aluno substituto não encontrado.",
                            "SUBSTITUTE_STUDENT_NOT_FOUND");

                    enrollment.CancellationOption = CancellationOption.SubstituteStudent;
                    enrollment.SubstituteStudentId = request.SubstituteStudentId;
                }
            }

            enrollment.IsActive = false;
            _unitOfWork.Enrollments.Update(enrollment);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
