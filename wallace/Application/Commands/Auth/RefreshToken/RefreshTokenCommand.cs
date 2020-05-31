using System;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Common.Interfaces;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Auth.Refresh
{
    public class RefreshTokenCommand : IRequest<TokenCollectionDto>
    {
        /// <summary>
        /// Refresh token given to the user.
        /// </summary>
        public string Jwt { get; set; }

        /// <summary>
        /// Expiry date given with the token. Doesn't actually get checked
        /// since that's already in the JWT token, but used to return it back
        /// to the user.
        /// </summary>
        public DateTime Expiry { get; set; }
    }

    /// <summary>
    /// Check's the refresh token validity and returns a new access token for
    /// the holder of the token.
    /// </summary>
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenCollectionDto>
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
        
        public async Task<TokenCollectionDto> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!_tokenChecker.IsValid(request.Jwt, _dateTime.UtcNow))
                throw new InvalidCredentialException();

            var tokenUserId = _tokenData.GetUserIdFrom(request.Jwt);
            if (tokenUserId == null)
                throw new InvalidCredentialException();

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(
                    u => u.Id == tokenUserId,
                    cancellationToken
                );
            
            if (user == null)
                throw new InvalidCredentialException();

            var refreshedToken = _tokenBuilder.BuildAccessToken(user);

            return new TokenCollectionDto
            {
                AccessToken = new TokenDto
                {
                    Jwt = refreshedToken.Jwt,
                    Expiry = _dateTime.UtcNow.AddMinutes(refreshedToken.Lifetime)
                },
                RefreshToken = new TokenDto
                {
                    Jwt = request.Jwt,
                    Expiry = request.Expiry
                }
            };
        }
    }
}