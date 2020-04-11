using System;
using System.IdentityModel.Tokens.Jwt;
using Wallace.Domain.Identity.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Domain.Identity
{
    public class TokenChecker : ITokenChecker
    {
        public bool IsValid(Token token, DateTime now)
        {
            if (token is null) return false;
            
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return now >= jwt.ValidFrom && now <= jwt.ValidTo;
        }
    }
}