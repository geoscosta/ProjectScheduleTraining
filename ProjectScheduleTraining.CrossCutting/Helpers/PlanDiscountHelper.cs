using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.CrossCutting.Helpers
{
    /// <summary>
    /// Helper responsável por calcular descontos nos planos fidelidade
    /// conforme definido no contrato do Estúdio Trinca.
    /// </summary>
    public static class PlanDiscountHelper
    {
        /// <summary>
        /// Retorna o percentual de desconto baseado no tipo do plano
        /// e no método de pagamento.
        /// Trimestral: 10% à vista / 5% cartão
        /// Semestral:  15% à vista / 5% cartão
        /// Anual:      20% à vista / 10% cartão
        /// </summary>
        public static decimal GetDiscountPercentage(PlanType planType, PaymentMethod paymentMethod)
        {
            var isCash = paymentMethod is PaymentMethod.Cash or PaymentMethod.Pix;

            return planType switch
            {
                PlanType.Quarterly      => isCash ? 10m : 5m,
                PlanType.SemiAnnual     => isCash ? 15m : 5m,
                PlanType.Annual         => isCash ? 20m : 10m,
                _                       => 0m
            };
        }

        /// <summary>
        /// Aplica o desconto sobre o valor do plano e retorna o preço final.
        /// </summary>
        public static decimal ApplyDiscount(decimal price, decimal discountPercentage)
            => price - (price * discountPercentage / 100m);

        /// <summary>
        /// Verifica se o tipo de plano é elegível para desconto de fidelidade.
        /// </summary>
        public static bool IsEligibleForDiscount(PlanType planType)
            => planType is PlanType.Quarterly or PlanType.SemiAnnual or PlanType.Annual;
    }
}
