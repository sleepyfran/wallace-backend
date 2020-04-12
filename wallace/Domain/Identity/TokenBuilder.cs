using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Entities;
using Wallace.Domain.Identity.Interfaces;
using Wallace.Domain.Common.Interfaces;
using Wallace.Domain.ValueObjects;

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
            return BuildToken(
                user,
                _configuration.TokenLifetime,
                _configuration.Key
            );
        }

        public Token BuildRefreshToken(User user)
        {
            return BuildToken(
                user,
                _configuration.RefreshTokenLifetime.Minutes,
                _configuration.RefreshKey
            );
        }
        
        private Token BuildToken(User user, Minutes expiresIn, string tokenKey)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var expires = _dateTime.UtcNow.AddMinutes(expiresIn);

            var jwtToken = new JwtSecurityToken(
                _configuration.Issuer,
                _configuration.Audience,
                expires: expires,
                signingCredentials: credentials,
                claims: new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }
            );

            var strToken = new JwtSecurityTokenHandler()
                .WriteToken(jwtToken);

            return new Token(strToken) { Lifetime = expiresIn };
        }
    }
}