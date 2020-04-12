using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Wallace.Domain.Identity.Interfaces;
using static Wallace.Domain.Identity.Model.Instance;

namespace Wallace.Domain.Identity
{
    public class IdentityLoader : IIdentityLoader
    {
        private readonly IIdentitySetter _identitySetter;

        public IdentityLoader(IIdentitySetter identitySetter)
        {
            _identitySetter = identitySetter;
        }
        
        public void LoadFrom(IEnumerable<Claim> claims)
        {
            var userClaim = claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userClaim == null)
            {
                _identitySetter.Set(Unknown);
                return;
            }

            if (!int.TryParse(userClaim.Value, out var userId))
            {
                _identitySetter.Set(Unknown);
                return;
            }

            _identitySetter.Set(new Model.UserIdentity
            {
                Id = userId
            });
        }
    }
}