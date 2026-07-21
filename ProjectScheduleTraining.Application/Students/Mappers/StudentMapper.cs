using ProjectScheduleTraining.Application.Students.DTOs;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Application.Students.Mappers
{
    /// <summary>
    /// Responsável por mapear a entidade Student para os DTOs de resposta.
    /// Centraliza toda a lógica de mapeamento do módulo de alunos.
    /// </summary>
    public static class StudentMapper
    {
        /// <summary>
        /// Mapeia a entidade Student para o DTO de resposta completa.
        /// </summary>
        public static StudentResponse ToResponse(Student student)
                => new(
                    student.Id,
                    student.Name,
                    student.Cpf,
                    student.Email,
                    student.Phone,
                    student.BirthDate,
                    student.Profession,
                    student.MaritalStatus,
                    student.IdentityDocument,
                    student.Street,
                    student.AddressNumber,
                    student.Complement,
                    student.District,
                    student.City,
                    student.State,
                    student.ZipCode,
                    student.GuardianName,
                    student.GuardianCpf,
                    student.EmergencyContact,
                    student.PhotoUrl,
                    student.RegistrationFeePaid,
                    student.HealthCertificateExpiresAt,
                    student.ImageRightsAccepted,
                    student.InternalRegulationAccepted,
                    student.Status,
                    student.StartDate,
                    student.CreatedAt,
                    student.UpdatedAt);

        /// <summary>
        /// Mapeia a entidade Student para o DTO de resposta resumida.
        /// Utilizado em listagens onde não são necessários todos os dados.
        /// </summary>
        public static StudentSummaryResponse ToSummaryResponse(Student student)
            => new(
                student.Id,
                student.Name,
                student.Email,
                student.Phone,
                student.Status);
    }
}
