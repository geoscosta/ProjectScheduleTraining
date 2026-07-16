namespace ProjectScheduleTraining.Application.Auth.DTOs
{
    /// <summary>
    /// DTO responsável por transportar o refresh token
    /// para renovação do access token.
    /// </summary>
    public record RefreshTokenRequest(string RefreshToken);
}
