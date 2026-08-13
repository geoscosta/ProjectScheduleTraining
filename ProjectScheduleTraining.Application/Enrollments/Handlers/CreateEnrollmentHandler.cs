using MediatR;
using ProjectScheduleTraining.Application.Enrollments.Commands;
using ProjectScheduleTraining.Application.Enrollments.DTOs;
using ProjectScheduleTraining.Application.Enrollments.Mappers;
using ProjectScheduleTraining.CrossCutting.Helpers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Enrollments.Handlers;

/// <summary>
/// Handler responsável por processar o comando de criação de uma nova matrícula.
/// Valida a existência e status do aluno, existência e status do plano,
/// calcula desconto automaticamente e persiste a matrícula.
/// </summary>
public class CreateEnrollmentHandler : IRequestHandler<CreateEnrollmentCommand, EnrollmentResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateEnrollmentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Processa o comando de criação da matrícula.
    /// Ordem de validação:
    /// 1. Aluno existe
    /// 2. Aluno está ativo
    /// 3. Plano existe e está ativo
    /// 4. Aluno não possui matrícula ativa
    /// 5. Calcula desconto por tipo de plano + método de pagamento
    /// 6. Persiste a matrícula
    /// </summary>
    public async Task<EnrollmentResponse> Handle(
        CreateEnrollmentCommand request,
        CancellationToken cancellationToken)
    {
        /// 1. Busca e valida o aluno.
        var student = await _unitOfWork.Students
            .GetByIdAsync(request.StudentId, cancellationToken);

        if (student is null)
            throw new DomainException(
                "Aluno não encontrado.",
                "STUDENT_NOT_FOUND");

        /// 2. Valida o status do aluno antes de buscar o plano.
        if (student.Status == StudentStatus.Inactive)
            throw new DomainException(
                "Não é possível matricular um aluno inativo.",
                "STUDENT_INACTIVE");

        if (student.Status == StudentStatus.Blocked)
            throw new DomainException(
                "Não é possível matricular um aluno bloqueado. Desbloqueie o aluno antes de matricular.",
                "STUDENT_BLOCKED");

        /// 3. Busca e valida o plano.
        var plan = await _unitOfWork.Plans
            .GetByIdAsync(request.PlanId, cancellationToken);

        if (plan is null)
            throw new DomainException(
                "Plano não encontrado.",
                "PLAN_NOT_FOUND");

        if (!plan.IsActive)
            throw new DomainException(
                "Plano inativo. Selecione um plano ativo.",
                "PLAN_INACTIVE");

        /// 4. Verifica se o aluno já possui matrícula ativa.
        var activeEnrollment = await _unitOfWork.Enrollments
            .GetByStudentIdAsync(request.StudentId, cancellationToken);

        if (activeEnrollment is not null)
            throw new DomainException(
                "Aluno já possui uma matrícula ativa.",
                "ENROLLMENT_ALREADY_EXISTS");

        /// 5. Calcula desconto automaticamente conforme tipo do plano
        /// e método de pagamento escolhido pelo aluno.
        var discountPercentage = PlanDiscountHelper.GetDiscountPercentage(
            plan.Type,
            request.PaymentMethod);

        var finalPrice = PlanDiscountHelper.ApplyDiscount(
            plan.Price,
            discountPercentage);

        /// 6. Cria e persiste a matrícula com todos os campos calculados.
        var enrollment = new Enrollment
        {
            StudentId = request.StudentId,
            PlanId = request.PlanId,
            StartDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddMonths(plan.DurationMonths),
            PaymentDueDay = request.PaymentDueDay,
            PaymentMethod = request.PaymentMethod,
            DiscountPercentage = discountPercentage,
            FinalPrice = finalPrice,
            IsActive = true
        };

        await _unitOfWork.Enrollments.AddAsync(enrollment, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        /// Popula as navegações para o mapper.
        enrollment.Student = student;
        enrollment.Plan = plan;

        return EnrollmentMapper.ToResponse(enrollment);
    }
}