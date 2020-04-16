using NUnit.Framework;
using Wallace.Application.Commands.Categories;

namespace Wallace.Tests.Application.Categories
{
    public class CategoryValidatorTests
    {
        private CategoryValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new CategoryValidator();
        }
        
        #region Name

        [Test]
        public void ShouldFailWithEmptyName()
        {
            _validator.ShouldHaveValidationErrorForEmpty(c => c.Name);
        }

        [Test]
        public void ShouldFailWithNamesMoreThan200Characters()
        {
            _validator.ShouldHaveValidationErrorForMoreThanCharacters(
                c => c.Name,
                200
            );
        }

        #endregion

        #region Emoji

        [Test]
        public void ShouldFailWithEmptyEmoji()
        {
            _validator.ShouldHaveValidationErrorForEmpty(c => c.Emoji);
        }

        #endregion
    }
}