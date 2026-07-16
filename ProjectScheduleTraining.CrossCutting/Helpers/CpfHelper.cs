namespace ProjectScheduleTraining.CrossCutting.Helpers
{
    /// <summary>
    /// Utilitário responsável por operações relacionadas ao CPF.
    /// </summary>
    public static class CpfHelper
    {
        /// <summary>
        /// Remove caracteres especiais do CPF, mantendo apenas os dígitos.
        /// </summary>
        public static string Clean(string cpf)
            => new(cpf.Where(char.IsDigit).ToArray());

        /// <summary>
        /// Formata o CPF no padrão 000.000.000-00.
        /// </summary>
        public static string Format(string cpf)
        {
            var digits = Clean(cpf);
            return $"{digits[..3]}.{digits[3..6]}.{digits[6..9]}-{digits[9..]}";
        }

        /// <summary>
        /// Valida o CPF verificando os dígitos verificadores.
        /// Remove caracteres especiais antes de validar.
        /// </summary>
        public static bool IsValid(string cpf)
        {
            var digits = Clean(cpf);

            if (digits.Length != 11) return false;
            if (digits.Distinct().Count() == 1) return false;

            var sum1 = digits.Take(9)
                .Select((d, i) => (d - '0') * (10 - i))
                .Sum();

            var remainder1 = sum1 % 11;
            var digit1 = remainder1 < 2 ? 0 : 11 - remainder1;

            var sum2 = digits.Take(10)
                .Select((d, i) => (d - '0') * (11 - i))
                .Sum();

            var remainder2 = sum2 % 11;
            var digit2 = remainder2 < 2 ? 0 : 11 - remainder2;

            return digit1 == (digits[9] - '0') && digit2 == (digits[10] - '0');
        }
    }
}
