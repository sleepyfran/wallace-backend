using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Common.Interfaces;
using Wallace.Domain.Identity.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Auth.Refresh
{
    public class RefreshTokenCommand : IRequest<Token>
    {
        /// <summary>
        /// Refresh token given to the user.
        /// </summary>
        public string RefreshToken { get; set; }
    }

    /// <summary>
    /// Check's the refresh token validity and returns a new access token for
    /// the holder of the token.
    /// </summary>
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Token>
    {
        private readonly IDbContext _dbContext;
        private readonly ITokenData _tokenData;
        private readonly ITokenChecker _tokenChecker;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IDateTime _dateTime;

        public RefreshTokenCommandHandler(
            IDbContext dbContext,
            ITokenData tokenData,
            ITokenChecker tokenChecker,
            ITokenBuilder tokenBuilder,
            IDateTime dateTime
        )
        {
            _dbContext = dbContext;
            _tokenData = tokenData;
            _tokenChecker = tokenChecker;
            _tokenBuilder = tokenBuilder;
            _dateTime = dateTime;
        }
        
        public async Task<Token> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!_tokenChecker.IsValid(request.RefreshToken, _dateTime.UtcNow))
                throw new InvalidCredentialException();

            var tokenUserId = _tokenData.GetUserIdFrom(request.RefreshToken);
            if (tokenUserId == null)
                throw new InvalidCredentialException();

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(
                    u => u.Id == tokenUserId,
                    cancellationToken
                );
            
            if (user == null)
                throw new InvalidCredentialException();

            return _tokenBuilder.BuildAccessToken(user);
        }
    }
}