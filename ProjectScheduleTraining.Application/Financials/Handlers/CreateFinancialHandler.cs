using MediatR;
using ProjectScheduleTraining.Application.Financials.Commands;
using ProjectScheduleTraining.Application.Financials.DTOs;
using ProjectScheduleTraining.Application.Financials.Mappers;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Financials.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de criação de uma nova cobrança financeira.
    /// Valida a existência do aluno antes de persistir a cobrança.
    /// </summary>
    public class CreateFinancialHandler : IRequestHandler<CreateFinancialCommand, FinancialResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFinancialHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de criação da cobrança financeira.
        /// Valida se o aluno existe antes de persistir.
        /// </summary>
        public async Task<FinancialResponse> Handle(
            CreateFinancialCommand request,
            CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.StudentId, cancellationToken);

            if (student is null)
                throw new DomainException(
                    "Aluno não encontrado.",
                    "STUDENT_NOT_FOUND");

            var financial = new Financial
            {
                StudentId = request.StudentId,
                EnrollmentId = request.EnrollmentId,
                Amount = request.Amount,
                DueDate = request.DueDate,
                Status = FinancialStatus.Pending,
                Description = request.Description?.Trim()
            };

            await _unitOfWork.Financials.AddAsync(financial, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            financial.Student = student;

            return FinancialMapper.ToResponse(financial);
        }
    }
}
