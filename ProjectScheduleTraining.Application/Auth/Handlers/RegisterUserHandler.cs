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
    /// Handler responsável por processar o comando de criação de um novo usuário.
    /// Valida duplicidade de e-mail e persiste o usuário com senha criptografada.
    /// </summary>
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserAuthResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;

        public RegisterUserHandler(
            IUnitOfWork unitOfWork,
            IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Processa o comando de criação do usuário.
        /// Verifica se o e-mail já está cadastrado antes de persistir.
        /// </summary>
        public async Task<UserAuthResponse> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            var emailExists = await _unitOfWork.Users
                .EmailExistsAsync(request.Email, cancellationToken);

            if (emailExists)
                throw new DomainException(
                    "Já existe um usuário cadastrado com esse e-mail.",
                    "USER_EMAIL_ALREADY_EXISTS");

            var user = new User
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLower(),
                PasswordHash = _passwordService.HashPassword(request.Password),
                Role = request.Role,
                IsActive = true,
                StudentId = request.StudentId
            };

            await _unitOfWork.Users.AddAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new UserAuthResponse(
                user.Id,
                user.Name,
                user.Email,
                user.Role);
        }
    }
}
