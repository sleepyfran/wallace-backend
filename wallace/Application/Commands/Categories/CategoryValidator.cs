using FluentValidation;
using Wallace.Application.Common.Dto;

namespace Wallace.Application.Commands.Categories
{
    public class CategoryValidator : AbstractValidator<CategoryDto>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(c => c.Emoji)
                .NotEmpty();
        }
    }
}