using FluentValidation;
using Wallace.Application.Common.Validators;

namespace Wallace.Application.Commands.Accounts.EditAccount
{
    public class EditAccountValidator : AbstractValidator<EditAccountCommand>
    {
        public EditAccountValidator()
        {
            Include(new AccountValidator());
            RuleFor(c => c).MustHaveMatchingIds();
        }
    }
}