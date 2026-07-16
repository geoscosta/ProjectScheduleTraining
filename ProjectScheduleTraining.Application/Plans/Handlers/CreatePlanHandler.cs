using MediatR;
using ProjectScheduleTraining.Application.Plans.Commands;
using ProjectScheduleTraining.Application.Plans.DTOs;
using ProjectScheduleTraining.Application.Plans.Mappers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Plans.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de criação de um novo plano.
    /// Valida duplicidade de nome e persiste o plano no banco de dados.
    /// </summary>
    public class CreatePlanHandler : IRequestHandler<CreatePlanCommand, PlanResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePlanHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de criação do plano.
        /// Verifica se já existe um plano com o mesmo nome antes de persistir.
        /// </summary>
        public async Task<PlanResponse> Handle(
            CreatePlanCommand request,
            CancellationToken cancellationToken)
        {
            var plans = await _unitOfWork.Plans
                .GetActivePlansAsync(cancellationToken);

            var nameExists = plans.Any(p =>
                p.Name.Equals(request.Name.Trim(),
                StringComparison.OrdinalIgnoreCase));

            if (nameExists)
                throw new DomainException(
                    "Já existe um plano cadastrado com esse nome.",
                    "PLAN_NAME_ALREADY_EXISTS");

            var plan = new Plan
            {
                Name = request.Name.Trim(),
                Type = request.Type,
                WeeklyFrequency = request.WeeklyFrequency,
                DurationMonths = request.DurationMonths,
                Price = request.Price,
                Description = request.Description?.Trim(),
                IsActive = true
            };

            await _unitOfWork.Plans.AddAsync(plan, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return PlanMapper.ToResponse(plan);
        }
    }
}
