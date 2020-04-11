using System.Linq;
using FluentValidation;
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
                .Must(BeUniqueEmail)
                .WithMessage("This email is already in use by another user");

            RuleFor(s => s.Password)
                .MinimumLength(8)
                .NotEmpty();

            RuleFor(s => s.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
        
        private bool BeUniqueEmail(string email)
        {
            return !_dbContext.Users
                .Any(u => u.Email == email);
        }
    }
}