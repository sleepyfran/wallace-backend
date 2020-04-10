namespace Wallace.Application.Common.Interfaces
{
    /// <summary>
    /// Abstraction over BCrypt to hash and verify passwords.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Returns a hashed copy of the given input.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string Hash(string input);
        
        /// <summary>
        /// Verifies that the given input is the same as the hashed password.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        bool Verify(string input, string hashedPassword);
    }
}