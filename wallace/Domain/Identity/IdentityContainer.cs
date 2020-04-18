using System;
using System.Threading.Tasks;
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
        
        public async Task<T> WithCurrentIdentity<T>(
            Func<IIdentity, Task<T>> wrappedFunc
        )
        {
            return await wrappedFunc(Get());
        }
        
        public async Task<T> WithCurrentIdentityId<T>(
            Func<Guid, Task<T>> wrappedFunc
        )
        {
            return await WithCurrentIdentity(async identity => 
                await wrappedFunc(identity.Id)
            );
        }
    }
}