using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Entities;

namespace Wallace.Domain.Identity.Interfaces
{
    /// <summary>
    /// Generic interface for creating JWT tokens.
    /// </summary>
    public interface ITokenBuilder
    {
        /// <summary>
        /// Creates a JWT token for a given user.
        /// </summary>
        Token BuildAccessToken(User user);
    }
}