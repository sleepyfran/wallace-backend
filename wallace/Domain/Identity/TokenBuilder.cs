using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Entities;
using Wallace.Domain.Identity.Interfaces;
using Wallace.Domain.Common.Interfaces;

namespace Wallace.Domain.Identity
{
    public class TokenBuilder : ITokenBuilder
    {
        private readonly JwtConfiguration _configuration;
        private readonly IDateTime _dateTime;

        public TokenBuilder(
            JwtConfiguration configuration,
            IDateTime dateTime
        )
        {
            _configuration = configuration;
            _dateTime = dateTime;
        }
        
        public Token BuildAccessToken(User user)
        {
            var lifetime = _configuration.TokenLifetime;
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var expires = _dateTime.UtcNow.AddMinutes(lifetime);

            var jwtToken = new JwtSecurityToken(
                _configuration.Issuer,
                _configuration.Audience,
                expires: expires,
                signingCredentials: credentials,
                claims: new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                }
            );

            var strToken = new JwtSecurityTokenHandler()
                .WriteToken(jwtToken);

            return new Token(strToken) { Lifetime = lifetime };
        }
    }
}