using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Auth.DTOs
{
    /// <summary>
    /// DTO responsável por transportar os dados
    /// para criação de um novo usuário no sistema.
    /// </summary>
    public record RegisterUserRequest(
        string Name,
        string Email,
        string Password,
        UserRole Role,
        Guid? StudentId);
}
