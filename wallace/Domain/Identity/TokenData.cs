using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Wallace.Domain.Identity.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Domain.Identity
{
    public class TokenData : ITokenData
    {
        public int? GetUserIdFrom(Token token)
        {
            var jwtToken = new JwtSecurityTokenHandler()
                .ReadJwtToken(token);

            return jwtToken.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c =>
                {
                    if (int.TryParse(c.Value, out var id))
                        return id;

                    return (int?)null;
                })
                .FirstOrDefault();
        }
    }
}