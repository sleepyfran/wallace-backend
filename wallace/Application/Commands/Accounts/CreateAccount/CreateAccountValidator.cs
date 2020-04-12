using FluentValidation;
using Wallace.Application.Common.Validators;

namespace Wallace.Application.Commands.Accounts.CreateAccount
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountValidator()
        {
            RuleFor(s => s.Name)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(s => s.Currency)
                .MustBeValidCurrency()
                .NotEmpty();

            RuleFor(s => s.Balance)
                .GreaterThan(0);
        }
    }
}