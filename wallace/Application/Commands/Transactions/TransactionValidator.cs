using System;
using FluentValidation;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Interfaces;
using Wallace.Application.Common.Validators;

namespace Wallace.Application.Commands.Transactions
{
    public class TransactionValidator : AbstractValidator<TransactionDto>
    {
        public TransactionValidator(IDbContext dbContext)
        {
            RuleFor(t => t.Amount)
                .GreaterThan(0)
                .NotEmpty();

            RuleFor(t => t.Currency)
                .MustBeValidCurrency()
                .NotEmpty();

            RuleFor(t => t.Date)
                .NotEmpty();

            RuleFor(t => t.Notes)
                .MaximumLength(200);

            RuleFor(t => t.Category)
                .MustBeValidIdReference(dbContext.Categories)
                .NotEmpty();

            RuleFor(t => t.Account)
                .MustBeValidIdReference(dbContext.Accounts)
                .NotEmpty();

            RuleFor(t => t.Payee)
                .MustBeValidIdReference(dbContext.Payees)
                .When(t => t.Payee != Guid.Empty);
        }
    }
}