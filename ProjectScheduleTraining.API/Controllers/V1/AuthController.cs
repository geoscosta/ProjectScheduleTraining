using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectScheduleTraining.Application.Auth.Commands;
using ProjectScheduleTraining.Application.Auth.DTOs;
using ProjectScheduleTraining.Domain.Enums;
using System.Security.Claims;

namespace ProjectScheduleTraining.API.Controllers.V1;

/// <summary>
/// Controller responsável por expor os endpoints de autenticação.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registra um novo usuário no sistema.
    /// Roles elevadas (Admin, Trainer, Receptionist) exigem que o solicitante
    /// seja um Administrador autenticado. A role Student pode ser registrada
    /// sem autenticação prévia (fluxo de auto-cadastro).
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserAuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        // Roles privilegiadas só podem ser criadas por um Admin autenticado
        var privilegedRoles = new[] { UserRole.Admin, UserRole.Trainer, UserRole.Receptionist };

        if (privilegedRoles.Contains(command.Role))
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized(new
                {
                    success = false,
                    errors = new[] { "Autenticação necessária para registrar usuários com perfil privilegiado." }
                });

            if (!User.IsInRole("Admin"))
                return Forbid();
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, result);
    }

    /// <summary>
    /// Realiza o login do usuário e retorna os tokens de acesso.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Renova o access token utilizando o refresh token.
    /// </summary>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Realiza o logout do usuário revogando todos os refresh tokens ativos.
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        if (userId is null)
            return Unauthorized();

        await _mediator.Send(new LogoutCommand(Guid.Parse(userId)), cancellationToken);
        return NoContent();
    }
}
