using Wallace.Application.Common.Interfaces;

namespace Wallace.Infrastructure
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        public bool Verify(string input, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(input, hashedPassword);
        }
    }
}