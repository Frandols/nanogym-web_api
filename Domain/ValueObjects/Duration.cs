using Domain.Enums;

namespace Domain.ValueObjects
{
    public sealed record Duration
    {
        public int Magnitude { get; }
        public DurationUnit Unit { get; }

        public Duration(int magnitude, DurationUnit unit)
        {
            if (magnitude <= 0)
                throw new ArgumentOutOfRangeException();

            Magnitude = magnitude;
            Unit = unit;
        }

        public DateTime AddTo(DateTime start)
        {
            return Unit switch
            {
                DurationUnit.Day => start.AddDays(Magnitude),
                DurationUnit.Week => start.AddDays(Magnitude * 7),
                DurationUnit.Month => start.AddMonths(Magnitude),
                DurationUnit.Year => start.AddYears(Magnitude),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override string ToString()
            => $"{Magnitude} {Unit}";
    }

}
