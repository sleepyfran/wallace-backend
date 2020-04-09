using FluentValidation;
using Wallace.Application.Common.Validators;

namespace Wallace.Application.Commands.Setup
{
    public class SetupValidator : AbstractValidator<SetupCommand>
    {
        public SetupValidator()
        {
            RuleFor(s => s.Email)
                .EmailAddress()
                .NotEmpty();

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
    }
}