using Domain.Enums;

namespace Domain.Entities
{
    public class PaymentOrder
    {
        public Guid Id { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentMean PaymentMean { get; private set; }
        public bool IsPaid { get; private set; }

        public PaymentOrder(CreatePaymentOrderInput input)
        {
            if (input.Amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(input), "Amount must be greater than zero.");

            Id = Guid.NewGuid();
            Amount = input.Amount;
            PaymentMean = input.PaymentMean;
            IsPaid = false;
        }

        public void Pay()
        {
            if (IsPaid)
                throw new InvalidOperationException("This payment order has already been used.");

            IsPaid = true;
        }
    }

    public sealed record CreatePaymentOrderInput(
        decimal Amount, 
        PaymentMean PaymentMean);
}
