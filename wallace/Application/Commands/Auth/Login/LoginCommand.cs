using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Exceptions;
using Wallace.Domain.Identity.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Auth.Login
{
    public class LoginCommand : IRequest<(Token, Token)>
    {
        /// <summary>
        /// Email of the user used to login.
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Password as inputted by the user.
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// Checks user's credentials and returns a token for API calls.
    /// </summary>
    public class LoginCommandHandler : IRequestHandler<LoginCommand, (Token, Token)>
    {
        private readonly IDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenBuilder _tokenBuilder;

        public LoginCommandHandler(
            IDbContext dbContext,
            IPasswordHasher passwordHasher,
            ITokenBuilder tokenBuilder
        )
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _tokenBuilder = tokenBuilder;
        }
        
        public async Task<(Token, Token)> Handle(
            LoginCommand request,
            CancellationToken cancellationToken
        )
        {
            var existingUser = await _dbContext.Users
                .FirstOrDefaultAsync(
                    u => u.Email == request.Email,
                    cancellationToken
                );
            
            if (existingUser == null)
                throw new UserNotFoundException();

            var passwordMatches = _passwordHasher.Verify(
                request.Password,
                existingUser.Password
            );
            
            if (!passwordMatches)
                throw new InvalidCredentialException();

            return (
                _tokenBuilder.BuildAccessToken(existingUser),
                _tokenBuilder.BuildRefreshToken(existingUser)
            );
        }
    }
}