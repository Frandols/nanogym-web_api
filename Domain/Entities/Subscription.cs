using Domain.Enums;

namespace Domain.Entities
{
    public class Subscription
    {
        public Guid Id { get; private set; }
        public Guid MemberId { get; private set; }
        public Plan Plan { get; private set; }
        public Guid GymId { get; private set; }
        public DateTime StartsAt { get; private set; }
        public DateTime EndsAt { get; private set; }
        private List<Payment> _payments;
        public IReadOnlyCollection<Payment> Payments => _payments;
        public bool IsPaid { get; private set; }

        public Subscription(CreateSubscriptionInput input)
        {
            if (input.Plan == null)
                throw new ArgumentNullException(nameof(input));

            if (input.StartsAt < DateTime.UtcNow)
                throw new ArgumentException("Start date cannot be in the past.", nameof(input));

            Id = Guid.NewGuid();
            MemberId = input.MemberId;
            Plan = input.Plan;
            GymId = input.GymId;
            StartsAt = input.StartsAt;
            EndsAt = input.Plan.Duration.AddTo(input.StartsAt);
            _payments = [];
            IsPaid = false;
        }

        public void AddPayment(CreatePaymentInput input)
        {
            if (IsPaid)
                throw new ArgumentException("Subscription is already paid.");

            var totalPaid = _payments.Sum(p => p.Amount) + input.Amount;

            if (totalPaid > Plan.Price)
                throw new InvalidOperationException("Payment exceeds the total price of the subscription.");

            if(totalPaid == Plan.Price)
                IsPaid = true;

            var payment = new Payment(input);

            _payments.Add(payment);
        }

        public SubscriptionStatus Status => CalculateStatusForDateTime(DateTime.UtcNow);

        private SubscriptionStatus CalculateStatusForDateTime(DateTime dateTime)
        {
            var endDate = Plan.Duration.AddTo(StartsAt).Date;

            if (dateTime.Date > endDate)
                return SubscriptionStatus.Expired;

            if (dateTime.Date == endDate)
                return SubscriptionStatus.ExpiresToday;

            if ((endDate - dateTime.Date).TotalDays <= 7)
                return SubscriptionStatus.ExpiresSoon;

            return SubscriptionStatus.UpToDate;
        }
    }

    public sealed record CreateSubscriptionInput(
        Guid MemberId, 
        Plan Plan, 
        Guid GymId, 
        DateTime StartsAt);
}
