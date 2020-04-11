using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Auth.SignUp
{
    public class SignUpCommand : IRequest<Token>
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
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, Token>
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
        
        public async Task<Token> Handle(
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

            return _tokenBuilder.BuildAccessToken(user);
        }
    }
}