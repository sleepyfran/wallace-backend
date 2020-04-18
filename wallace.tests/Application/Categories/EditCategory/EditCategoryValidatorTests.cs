using System;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Wallace.Application.Commands.Categories.EditCategory;

namespace Wallace.Tests.Application.Accounts.CreateAccount.EditAccount
{
    public class EditCategoryValidatorTests
    {
        private EditCategoryValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new EditCategoryValidator();
        }

        [Test]
        public void ShouldNotFailIfIdsMatch()
        {
            var id = Guid.NewGuid();
            var result = _validator.TestValidate(new EditCategoryCommand
            {
                Id = id,
                QueryId = id,
                Name = "Matching IDs",
                Emoji = "😀",
                Owner = Guid.NewGuid()
            });
            
            result.ShouldNotHaveAnyValidationErrors();
        }
        
        [Test]
        public void ShouldFailWhenIdsDoNotMatch()
        {
            var result = _validator.TestValidate(new EditCategoryCommand
            {
                Id = Guid.NewGuid(),
                QueryId = Guid.NewGuid(),
                Name = "Matching IDs",
                Emoji = "🔥",
                Owner = Guid.NewGuid()
            });

            result.ShouldHaveValidationErrorFor(
                c => c.QueryId
            );
        }
    }
}