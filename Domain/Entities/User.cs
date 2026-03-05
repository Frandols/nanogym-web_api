using Domain.Enums;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public bool HasIntegration { get; private set; }

        public User(CreateUserInput input)
        {
            ValidateName(input.Name);
            ValidateEmail(input.Email);
            ValidatePasswordHash(input.PasswordHash);

            Id = Guid.NewGuid();
            Name = input.Name;
            Email = input.Email;
            PasswordHash = input.PasswordHash;
            HasIntegration = false;
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

        static private void ValidatePasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be null or empty.", nameof(passwordHash));
        }

        public bool IsCorrectPassword(string passwordHash)
            => PasswordHash == passwordHash;

        public void ChangePassword(string currentPasswordHash, string newPasswordHash)
        {
            if (!IsCorrectPassword(currentPasswordHash))
                throw new UnauthorizedAccessException("Current password is incorrect.");

            ValidatePasswordHash(newPasswordHash);
            
            PasswordHash = newPasswordHash;
        }

        public void Update(UpdateUserInput input)
        {
            ValidateName(input.Name);
            ValidateEmail(input.Email);

            Name = input.Name;
            Email = input.Email;
        }

        public void EnableIntegration()
            => HasIntegration = true;

        public void DisableIntegration()
            => HasIntegration = false;

        public bool CanBePaidBy(PaymentMean mean)
        {
            if (mean == PaymentMean.Cash) return true;

            if (mean == PaymentMean.QRCode) return HasIntegration;

            throw new ArgumentException("Payment mean not configured.", nameof(mean));
        }
    }

    public sealed record CreateUserInput(
        string Name, 
        string Email,
        string PasswordHash);

    public sealed record UpdateUserInput(
        string Name, 
        string Email);
}
