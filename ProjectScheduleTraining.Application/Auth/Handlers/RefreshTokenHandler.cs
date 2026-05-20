using MediatR;
using ProjectScheduleTraining.Application.Auth.Commands;
using ProjectScheduleTraining.Application.Auth.DTOs;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Auth.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de renovação do access token.
    /// Valida o refresh token e gera novos tokens de acesso.
    /// </summary>
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public RefreshTokenHandler(
            IUnitOfWork unitOfWork,
            IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        /// <summary>
        /// Processa o comando de renovação do access token.
        /// Revoga o refresh token atual e gera um novo par de tokens.
        /// </summary>
        public async Task<AuthResponse> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var refreshToken = await _unitOfWork.RefreshTokens
                .GetByTokenAsync(request.RefreshToken, cancellationToken);

            if (refreshToken is null || !refreshToken.IsValid())
                throw new DomainException(
                    "Refresh token inválido ou expirado.",
                    "INVALID_REFRESH_TOKEN");

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;

            var newAccessToken = _authService.GenerateAccessToken(refreshToken.User!);
            var newRefreshToken = _authService.GenerateRefreshToken();

            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = refreshToken.UserId,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(
                    _authService.GetRefreshTokenExpirationDays())
            };

            await _unitOfWork.RefreshTokens.AddAsync(newRefreshTokenEntity, cancellationToken);
            _unitOfWork.RefreshTokens.Update(refreshToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new AuthResponse(
                newAccessToken,
                newRefreshToken,
                DateTime.UtcNow.AddMinutes(_authService.GetAccessTokenExpirationMinutes()),
                new UserAuthResponse(
                    refreshToken.User!.Id,
                    refreshToken.User.Name,
                    refreshToken.User.Email,
                    refreshToken.User.Role));
        }
    }
}
