namespace ProjectScheduleTraining.Application.Auth.DTOs
{
    /// <summary>
    /// DTO responsável por transportar os dados de login do usuário.
    /// </summary>
    public record LoginRequest(
        string Email,
        string Password);
}
