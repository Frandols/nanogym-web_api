using Application.Abstractions.Hashing;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Hashing
{
    public sealed class IdentityHasher : IHasher
    {
        private readonly PasswordHasher<object> _hasher = new();

        public string Hash(string input)
        {
            return _hasher.HashPassword(null!, input);
        }

        public bool Verify(string input, string hash)
        {
            var result = _hasher.VerifyHashedPassword(null!, hash, input);

            return result == PasswordVerificationResult.Success
                   || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}