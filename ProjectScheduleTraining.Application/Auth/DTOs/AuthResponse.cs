namespace ProjectScheduleTraining.Application.Auth.DTOs
{
    /// <summary>
    /// DTO de resposta da autenticação.
    /// Retorna o access token, refresh token e dados básicos do usuário.
    /// </summary>
    public record AuthResponse(
        string AccessToken,
        string RefreshToken,
        DateTime ExpiresAt,
        UserAuthResponse User);
}
