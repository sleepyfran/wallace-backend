using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Auth.SignUp
{
    public class SignUpCommand : IRequest<AuthDto>
    {
        /// <summary>
        /// Name of the user.
        /// </summary>
        public string Name { get; set; }
        
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
    /// Creates a new user and returns a JWT token for authenticating future
    /// API calls.
    /// </summary>
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, AuthDto>
    {
        private readonly IDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenBuilder _tokenBuilder;

        public SignUpCommandHandler(
            IDbContext dbContext,
            IPasswordHasher passwordHasher,
            ITokenBuilder tokenBuilder
        )
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _tokenBuilder = tokenBuilder;
        }
        
        public async Task<AuthDto> Handle(
            SignUpCommand request,
            CancellationToken cancellationToken
        )
        {
            var hashedPassword = _passwordHasher.Hash(request.Password);
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = hashedPassword
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new AuthDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Token = new TokenDto
                {
                    AccessToken = _tokenBuilder.BuildAccessToken(user),
                    RefreshToken = _tokenBuilder.BuildRefreshToken(user)
                }
            };
        }
    }
}