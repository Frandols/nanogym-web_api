namespace Domain.ValueObjects
{
    public sealed class Administrator
    {
        public Guid UserId { get; }

        public Administrator(Guid userId)
        {
            UserId = userId;
        }
    }
}
