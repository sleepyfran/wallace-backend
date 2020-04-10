using System.Linq;
using FluentValidation;
using Wallace.Application.Common.Interfaces;
using Wallace.Application.Common.Validators;

namespace Wallace.Application.Commands.Setup
{
    public class SetupValidator : AbstractValidator<SetupCommand>
    {
        private readonly IDbContext _dbContext;

        public SetupValidator(IDbContext dbContext)
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

            RuleFor(s => s.AccountName)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(s => s.BaseCurrency)
                .MustBeValidCurrency()
                .NotEmpty();

            RuleFor(s => s.CategorySelection)
                .NotNull();

            RuleFor(s => s.InitialBalance)
                .GreaterThan(0);
        }

        private bool BeUniqueEmail(string email)
        {
            return !_dbContext.Users
                .Any(u => u.Email == email);
        }
    }
}