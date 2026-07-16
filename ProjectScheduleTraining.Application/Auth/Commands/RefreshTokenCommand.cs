using MediatR;
using ProjectScheduleTraining.Application.Auth.DTOs;

namespace ProjectScheduleTraining.Application.Auth.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para renovação do access token utilizando o refresh token.
    /// </summary>
    public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponse>;
}
