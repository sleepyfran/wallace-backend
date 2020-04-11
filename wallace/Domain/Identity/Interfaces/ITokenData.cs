using Wallace.Domain.Identity.Entities;

namespace Wallace.Domain.Identity.Interfaces
{
    /// <summary>
    /// Methods for accessing data from tokens.
    /// </summary>
    public interface ITokenData
    {
        int? GetUserIdFrom(Token token);
    }
}