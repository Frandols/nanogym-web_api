namespace Domain.Entities
{
    public class Session
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string TokenHash { get; private set; }
        public string IpAddress { get; private set; }
        public string UserAgent { get; private set; }
        public bool IsClosed { get; private set; }

        public Session(CreateSessionInput input)
        {
            if(string.IsNullOrEmpty(input.TokenHash))
                throw new ArgumentException("Token hash cannot be null or empty.", nameof(input));

            if (!System.Net.IPAddress.TryParse(input.IpAddress, out _))
                throw new ArgumentException("Invalid IP address format.", nameof(input));

            if (string.IsNullOrWhiteSpace(input.UserAgent))
                throw new ArgumentException("User agent cannot be null or empty.", nameof(input));

            Id = Guid.NewGuid();
            UserId = input.UserId;
            TokenHash = input.TokenHash;
            IpAddress = input.IpAddress;
            UserAgent = input.UserAgent;
            IsClosed = false;
        }

        public void Close()
        {
            if (IsClosed)
                throw new InvalidOperationException("Session is already closed.");

            IsClosed = true;
        }
    }

    public sealed record CreateSessionInput(
        Guid UserId, 
        string TokenHash,
        string IpAddress, 
        string UserAgent);
}
