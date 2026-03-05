namespace Domain.Entities
{
    public class InvitationToAdministrate
    {
        public Guid Id { get; }
        public Guid GymId { get; }
        public bool IsUsed { get; private set; }
        public TimeSpan ValidFor { get; }
        public DateTime IssueDate { get; }

        public InvitationToAdministrate(CreateInvitationToAdministrateInput input)
        {
            Id = Guid.NewGuid();
            GymId = input.GymId;
            ValidFor = input.ValidFor;

            IssueDate = DateTime.UtcNow;
        }

        public void Use()
        {
            if (IsUsed)
                throw new InvalidOperationException("This invitation has already been used.");

            if (DateTime.UtcNow > IssueDate.Add(ValidFor))
                throw new InvalidOperationException("This invitation has expired.");

            IsUsed = true;
        }
    }

    public sealed record CreateInvitationToAdministrateInput(
        Guid GymId,
        TimeSpan ValidFor);
}
