using Wallace.Domain.Identity.Enums;

namespace Wallace.Domain.Identity.Model
{
    /// <summary>
    /// Represents a not logged in user. Used for endpoints that do not
    /// require authentication or when a token has expired. Generally the
    /// identity will not be used in this cases since the endpoints that
    /// are protected won't be reachable and the endpoints that do not require
    /// protection don't use the identity.
    /// </summary>
    public class Unknown : IIdentity
    {
        public int Id => -1;
        public IdentityType Type => IdentityType.Unknown;
    }

    public static class Instance
    {
        public static readonly Unknown Unknown = new Unknown();
    }
}