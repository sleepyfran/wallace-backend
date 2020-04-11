using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Wallace.Application.Common.Interfaces;

namespace Wallace.Application.Commands.Auth.SignUp
{
    public class SignUpValidator : AbstractValidator<SignUpCommand>
    {
        private readonly IDbContext _dbContext;

        public SignUpValidator(IDbContext dbContext)
        {
            _dbContext = dbContext;
            
            RuleFor(s => s.Email)
                .EmailAddress()
                .NotEmpty()
                .MustAsync(async (e, ct) => 
                    await BeUniqueEmail(e, ct)
                )
                .WithMessage("This email is already in use by another user");

            RuleFor(s => s.Password)
                .MinimumLength(8)
                .NotEmpty();

            RuleFor(s => s.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
        
        private async Task<bool> BeUniqueEmail(
            string email,
            CancellationToken cancellationToken
        )
        {
            var exists = await _dbContext.Users
                .AnyAsync(
                    u => u.Email == email,
                    cancellationToken
                );

            return !exists;
        }
    }
}