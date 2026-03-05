using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Plan
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public Duration Duration { get; private set; }

        public Plan(CreatePlanInput input)
        {
            ValidateName(input.Name);
            ValidatePrice(input.Price);
            ValidateDuration(input.Duration);

            Id = Guid.NewGuid();
            Name = input.Name;
            Price = input.Price;
            Duration = input.Duration;
        }

        static private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Plan name cannot be null or empty.", nameof(name));
        }

        static private void ValidatePrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
        }

        static private void ValidateDuration(Duration duration)
        {
            if (duration == null)
                throw new ArgumentNullException(nameof(duration));
        }

        public void Update(UpdatePlanInput input)
        {
            ValidateName(input.Name);
            ValidatePrice(input.Price);
            ValidateDuration(input.Duration);

            Name = input.Name;
            Price = input.Price;
            Duration = input.Duration;
        }
    }

    public sealed record CreatePlanInput(
        string Name, 
        decimal Price, 
        Duration Duration);

    public sealed record UpdatePlanInput(
        string Name, 
        decimal Price, 
        Duration Duration);
}
