using System;

namespace Wallace.Application.Common.Dto
{
    public class AuthDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public TokenDto Token { get; set; }
    }
}
