using FluentValidation;

namespace Wallace.Application.Commands.Accounts.EditAccount
{
    public class EditAccountValidator : AbstractValidator<EditAccountCommand>
    {
        public EditAccountValidator()
        {
            Include(new AccountValidator());
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (c.QueryId != c.Id)
                        context.AddFailure(
                            "QueryId",
                            "The ID given in the URL should match the one given in the body"
                        );
                });
        }
    }
}