using MediatR;
using ProjectScheduleTraining.Application.Auth.DTOs;

namespace ProjectScheduleTraining.Application.Auth.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para autenticação de um usuário no sistema.
    /// </summary>
    public record LoginCommand(
        string Email,
        string Password) : IRequest<AuthResponse>;
}
