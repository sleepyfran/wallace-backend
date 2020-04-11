using System;
using Wallace.Domain.Identity.Entities;

namespace Wallace.Domain.Identity.Interfaces
{
    /// <summary>
    /// Generic interface for checking JWT tokens.
    /// </summary>
    public interface ITokenChecker
    {
        bool IsValid(Token token, DateTime now);
    }
}