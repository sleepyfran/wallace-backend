using System;
using Wallace.Domain.Identity.Entities;

namespace Wallace.Application.Common.Dto
{
    public class TokenDto
    {
        public Token AccessToken { get; set; }
        public Token RefreshToken { get; set; }
    }
}
