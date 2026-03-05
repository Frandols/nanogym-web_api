namespace Domain.Entities
{
    public class Member
    {
        public Guid Id { get; private set; }
        public Guid GymId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;

        public Member(CreateMemberInput input)
        {
            ValidateName(input.Name);
            ValidateEmail(input.Email);

            Id = Guid.NewGuid();
            GymId = input.GymId;
            Name = input.Name;
            Email = input.Email;
        }

        static private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("User name cannot be null or empty.", nameof(name));
        }

        static private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("User email cannot be null or empty.", nameof(email));

            if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email))
                throw new ArgumentException("Invalid email format.", nameof(email));
        }

        public void Update(UpdateMemberInput input)
        {
            ValidateName(input.Name);
            ValidateEmail(input.Email);

            Name = input.Name;
            Email = input.Email;
        }
    }

    public sealed record CreateMemberInput(
        Guid GymId, 
        string Name, 
        string Email);

    public sealed record UpdateMemberInput(
        string Name,
        string Email);
}
