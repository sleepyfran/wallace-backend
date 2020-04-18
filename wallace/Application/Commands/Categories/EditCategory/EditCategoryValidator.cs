using FluentValidation;
using Wallace.Application.Common.Validators;

namespace Wallace.Application.Commands.Categories.EditCategory
{
    public class EditCategoryValidator : AbstractValidator<EditCategoryCommand>
    {
        public EditCategoryValidator()
        {
            Include(new CategoryValidator());
            RuleFor(c => c).MustHaveMatchingIds();
        }
    }
}