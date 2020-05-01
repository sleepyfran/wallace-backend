using FluentValidation;
using Wallace.Application.Common.Interfaces;
using Wallace.Application.Common.Validators;

namespace Wallace.Application.Commands.Transactions.EditTransaction
{
    public class EditTransactionValidator : AbstractValidator<EditTransactionCommand>
    {
        public EditTransactionValidator(IDbContext context)
        {
            Include(new TransactionValidator(context));
            RuleFor(c => c).MustHaveMatchingIds();
        }
    }
}