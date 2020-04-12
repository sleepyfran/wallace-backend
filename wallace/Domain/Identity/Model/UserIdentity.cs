using System;
using Wallace.Domain.Identity.Enums;

namespace Wallace.Domain.Identity.Model
{
    /// <summary>
    /// Representation of an user once its identity has been verified.
    /// </summary>
    public class UserIdentity : IIdentity
    {
        public Guid Id { get; set; }
        public IdentityType Type => IdentityType.User;
    }
}