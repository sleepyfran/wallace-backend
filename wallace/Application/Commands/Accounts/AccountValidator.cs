using FluentValidation;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Validators;

namespace Wallace.Application.Commands.Accounts
{
    public class AccountValidator : AbstractValidator<AccountDto>
    {
        public AccountValidator()
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