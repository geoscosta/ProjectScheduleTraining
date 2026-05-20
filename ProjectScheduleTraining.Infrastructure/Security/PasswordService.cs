using ProjectScheduleTraining.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ProjectScheduleTraining.Infrastructure.Security
{
    /// <summary>
    /// Serviço responsável por criar e verificar hashes de senhas.
    /// Utiliza PBKDF2 com SHA256 para garantir segurança no armazenamento.
    /// </summary>
    public class PasswordService : IPasswordService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        /// <summary>
        /// Gera o hash de uma senha em texto puro.
        /// Utiliza salt aleatório para evitar ataques de rainbow table.
        /// </summary>
        public string HashPassword(string password)
        {
            var salt = new byte[SaltSize];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            var result = new byte[SaltSize + HashSize];
            Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, result, SaltSize, HashSize);

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// Verifica se uma senha em texto puro corresponde ao hash armazenado.
        /// Extrai o salt do hash armazenado e recalcula para comparação.
        /// </summary>
        public bool VerifyPassword(string password, string passwordHash)
        {
            var hashBytes = Convert.FromBase64String(passwordHash);

            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }

            return true;
        }
    }
}
