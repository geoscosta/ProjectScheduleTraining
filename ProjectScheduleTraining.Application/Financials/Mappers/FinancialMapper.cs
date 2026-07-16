using ProjectScheduleTraining.Application.Financials.DTOs;
using ProjectScheduleTraining.Application.Students.Mappers;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Application.Financials.Mappers
{
    /// <summary>
    /// Responsável por mapear a entidade Financial para os DTOs de resposta.
    /// Centraliza toda a lógica de mapeamento do módulo financeiro.
    /// </summary>
    public static class FinancialMapper
    {
        /// <summary>
        /// Mapeia a entidade Financial para o DTO de resposta completa.
        /// Inclui os dados resumidos do aluno quando disponíveis.
        /// </summary>
        public static FinancialResponse ToResponse(Financial financial)
            => new(
                financial.Id,
                financial.StudentId,
                financial.EnrollmentId,
                financial.Amount,
                financial.DueDate,
                financial.PaymentDate,
                financial.Status,
                financial.Description,
                financial.PaymentProof,
                financial.CreatedAt,
                financial.UpdatedAt,
                financial.Student is not null
                    ? StudentMapper.ToSummaryResponse(financial.Student)
                    : null);

        /// <summary>
        /// Mapeia a entidade Financial para o DTO de resposta resumida.
        /// Utilizado em listagens e controle de inadimplência.
        /// </summary>
        public static FinancialSummaryResponse ToSummaryResponse(Financial financial)
            => new(
                financial.Id,
                financial.StudentId,
                financial.Amount,
                financial.DueDate,
                financial.Status);
    }
}
