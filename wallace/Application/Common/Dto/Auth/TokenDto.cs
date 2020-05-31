using System;
using Wallace.Domain.Identity.Entities;

namespace Wallace.Application.Common.Dto
{
    public class TokenDto
    {
        public string Jwt { get; set; }
        public DateTime Expiry { get; set; }
    }

    public class TokenCollectionDto
    {
        public TokenDto AccessToken { get; set; }
        public TokenDto RefreshToken { get; set; }

        public static TokenCollectionDto CreateFor(
            DateTime now,
            Token access,
            Token refresh
        )
        {
            return new TokenCollectionDto
            {
                AccessToken = new TokenDto
                {
                    Jwt = access.Jwt,
                    Expiry = now.AddMinutes(access.Lifetime)
                },
                RefreshToken = new TokenDto
                {
                    Jwt = refresh.Jwt,
                    Expiry = now.AddMinutes(refresh.Lifetime)
                }
            };
        }
    }
}
