using MediatR;
using ProjectScheduleTraining.Application.Auth.Commands;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Auth.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de logout.
    /// Revoga todos os refresh tokens ativos do usuário.
    /// </summary>
    public class LogoutHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogoutHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de logout do usuário.
        /// Revoga todos os refresh tokens ativos para invalidar sessões abertas.
        /// </summary>
        public async Task Handle(
            LogoutCommand request,
            CancellationToken cancellationToken)
        {
            await _unitOfWork.RefreshTokens
                .RevokeAllByUserIdAsync(request.UserId, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
