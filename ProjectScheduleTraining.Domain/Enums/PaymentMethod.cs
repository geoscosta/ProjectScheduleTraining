namespace ProjectScheduleTraining.Domain.Enums
{
    /// <summary>
    /// Enum responsável por representar o método de pagamento.
    /// O método impacta diretamente no desconto dos planos fidelidade.
    /// </summary>
    public enum PaymentMethod
    {
        /// Pagamento à vista — desconto maior.
        Cash = 1,

        /// Pagamento no cartão — desconto menor.
        Card = 2,

        /// PIX — equivalente a à vista.
        Pix = 3
    }
}
