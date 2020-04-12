using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Wallace.Domain.Identity.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Domain.Identity
{
    public class TokenData : ITokenData
    {
        public Guid GetUserIdFrom(Token token)
        {
            var jwtToken = new JwtSecurityTokenHandler()
                .ReadJwtToken(token);

            return jwtToken.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => 
                    Guid.TryParse(c.Value, out var id) 
                        ? id 
                        : Guid.Empty
                )
                .FirstOrDefault();
        }
    }
}