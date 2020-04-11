using System.Threading;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Wallace.Application.Commands.Auth.SignUp;
using Wallace.Domain.Entities;

namespace Wallace.Tests.Application.SignUp
{
    public class SignUpValidatorTest : BaseTest
    {
        private SignUpValidator _validator;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            _validator = new SignUpValidator(DbContext);
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

        [Test]
        public async Task ShouldErrorWithDuplicatedEmail()
        {
            DbContext.Users.Add(new User {Email = "test@test.com"});
            await DbContext.SaveChangesAsync(CancellationToken.None);

            _validator.ShouldHaveValidationErrorFor(
                c => c.Email,
                "test@test.com"
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
    }
}