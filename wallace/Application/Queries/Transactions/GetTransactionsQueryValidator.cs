using FluentValidation;

namespace Wallace.Application.Queries.Transactions
{
    public class GetTransactionsQueryValidator
        : AbstractValidator<GetTransactionsQuery>
    {
        public GetTransactionsQueryValidator()
        {
            RuleFor(q => q.Month)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(12);

            RuleFor(q => q.Year)
                .GreaterThanOrEqualTo(2000)
                .LessThanOrEqualTo(2200);
        }
    }
}