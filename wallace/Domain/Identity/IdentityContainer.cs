using Wallace.Domain.Identity.Model;

namespace Wallace.Domain.Identity.Interfaces
{
    /// <summary>
    /// Allows setting and retrieving the user identity.
    /// </summary>
    public class IdentityContainer : IIdentitySetter, IIdentityAccessor
    {
        private IIdentity _identity;
        
        public void Set(IIdentity identity)
        {
            _identity = identity;
        }

        public IIdentity Get() => _identity;
    }
}