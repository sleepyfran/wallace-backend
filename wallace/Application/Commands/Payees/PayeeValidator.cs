using FluentValidation;
using Wallace.Application.Common.Dto;

namespace Wallace.Application.Commands.Payees
{
    public class PayeeValidator :  AbstractValidator<PayeeDto>
    {
        public PayeeValidator()
        {
            RuleFor(c => c.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}