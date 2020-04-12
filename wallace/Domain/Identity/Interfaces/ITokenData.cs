using System;
using Wallace.Domain.Identity.Entities;

namespace Wallace.Domain.Identity.Interfaces
{
    /// <summary>
    /// Methods for accessing data from tokens.
    /// </summary>
    public interface ITokenData
    {
        Guid GetUserIdFrom(Token token);
    }
}