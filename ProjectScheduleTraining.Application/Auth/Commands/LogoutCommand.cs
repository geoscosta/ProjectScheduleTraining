using MediatR;

namespace ProjectScheduleTraining.Application.Auth.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para logout do usuário, revogando todos os refresh tokens ativos.
    /// </summary>
    public record LogoutCommand(Guid UserId) : IRequest;
}
