using MediatR;
using ProjectScheduleTraining.Application.Auth.DTOs;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Auth.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para criação de um novo usuário no sistema.
    /// </summary>
    public record RegisterUserCommand(
        string Name,
        string Email,
        string Password,
        UserRole Role,
        Guid? StudentId) : IRequest<UserAuthResponse>;
}
