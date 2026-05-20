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
    /// Handler responsável por processar o comando de login.
    /// Valida as credenciais e gera os tokens de acesso.
    /// </summary>
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IPasswordService _passwordService;

        public LoginHandler(
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Processa o comando de login do usuário.
        /// Valida as credenciais, gera o access token e o refresh token.
        /// </summary>
        public async Task<AuthResponse> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users
                .GetByEmailWithTokensAsync(request.Email, cancellationToken);

            if (user is null || !_passwordService.VerifyPassword(request.Password, user.PasswordHash))
                throw new DomainException(
                    "E-mail ou senha inválidos.",
                    "INVALID_CREDENTIALS");

            if (!user.IsActive)
                throw new DomainException(
                    "Usuário inativo. Entre em contato com o administrador.",
                    "USER_INACTIVE");

            var accessToken = _authService.GenerateAccessToken(user);
            var refreshToken = _authService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(
                    _authService.GetRefreshTokenExpirationDays())
            };

            await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new AuthResponse(
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddMinutes(_authService.GetAccessTokenExpirationMinutes()),
                new UserAuthResponse(user.Id, user.Name, user.Email, user.Role));
        }
    }
}
