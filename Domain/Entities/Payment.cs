using Domain.Enums;

namespace Domain.Entities
{
    public class Payment
    {
        public decimal Amount { get; }
        public PaymentMean Mean { get; }

        public Payment(CreatePaymentInput input)
        {
            if(input.Amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(input));

            Amount = input.Amount;
            Mean = input.Mean;
        }
    }

    public sealed record CreatePaymentInput(
        decimal Amount, 
        PaymentMean Mean);
}
