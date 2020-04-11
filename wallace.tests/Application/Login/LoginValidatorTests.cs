using FluentValidation.TestHelper;
using NUnit.Framework;
using Wallace.Application.Commands.Auth.Login;

namespace Wallace.Tests.Application.Login
{
    public class LoginValidatorTest : BaseTest
    {
        private LoginValidator _validator;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            _validator = new LoginValidator();
        }

        #region Email

        [Test]
        public void ShouldErrorWithEmptyEmail()
        {
            _validator.ShouldHaveValidationErrorForEmpty(c => c.Email);
        }

        [Test]
        public void ShouldErrorWithInvalidEmail(
            [Values("invalid", "invalid@", "invalid@invalid", "@")]
            string input
        )
        {
            _validator.ShouldHaveValidationErrorFor(
                c => c.Email,
                input
            );
        }

        #endregion

        #region Password

        [Test]
        public void ShouldFailWithEmptyPassword()
        {
            _validator.ShouldHaveValidationErrorForEmpty(s => s.Password);
        }

        [Test]
        public void ShouldFailWithPasswordsLessThan8Characters()
        {
            _validator.ShouldHaveValidationErrorForLessThanCharacters(
                c => c.Password,
                8
            );
        }

        #endregion
    }
}