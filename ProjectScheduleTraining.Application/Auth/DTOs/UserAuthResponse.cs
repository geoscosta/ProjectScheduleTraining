using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Auth.DTOs
{
    /// <summary>
    /// DTO com os dados básicos do usuário autenticado.
    /// Retornado junto com o token no processo de login.
    /// </summary>
    public record UserAuthResponse(
        Guid Id,
        string Name,
        string Email,
        UserRole Role);
}
