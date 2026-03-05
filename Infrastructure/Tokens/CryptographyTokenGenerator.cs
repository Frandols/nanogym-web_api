using Application.Abstractions.Tokens;
using System.Security.Cryptography;

namespace Infrastructure.Tokens
{
    public class CryptographyTokenGenerator : ITokenGenerator
    {
        public string Execute()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);

            return Convert.ToBase64String(bytes);
        }
    }
}
