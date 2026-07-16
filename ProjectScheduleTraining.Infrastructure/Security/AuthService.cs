using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProjectScheduleTraining.Infrastructure.Security
{
    /// <summary>
    /// Serviço responsável pela geração e validação de tokens JWT.
    /// Implementa a interface IAuthService definida no Domain.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;

        public AuthService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Gera um access token JWT para o usuário informado.
        /// Inclui as claims de identificação, e-mail e perfil do usuário.
        /// </summary>
        public string GenerateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Gera um refresh token aleatório e seguro usando criptografia.
        /// </summary>
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        /// <summary>
        /// Retorna o tempo de expiração do access token em minutos.
        /// </summary>
        public int GetAccessTokenExpirationMinutes()
            => _jwtSettings.AccessTokenExpirationMinutes;

        /// <summary>
        /// Retorna o tempo de expiração do refresh token em dias.
        /// </summary>
        public int GetRefreshTokenExpirationDays()
            => _jwtSettings.RefreshTokenExpirationDays;
    }
}
