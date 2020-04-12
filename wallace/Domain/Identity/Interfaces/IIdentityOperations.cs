using System.Collections.Generic;
using System.Security.Claims;
using Wallace.Domain.Identity.Model;

namespace Wallace.Domain.Identity.Interfaces
{
    /// <summary>
    /// Sets the identity once it's been verified. The setter should NEVER be
    /// called with an invalid ID.
    /// </summary>
    public interface IIdentitySetter
    {
        void Set(IIdentity identity);
    }

    /// <summary>
    /// Retrieves the identity from the container.
    /// </summary>
    public interface IIdentityAccessor
    {
        IIdentity Get();
    }

    /// <summary>
    /// Retrieves the user identity from the given claims and loads them
    /// into the container.
    /// </summary>
    public interface IIdentityLoader
    {
        void LoadFrom(IEnumerable<Claim> claims);
    }
}