using System.Threading;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Wallace.Application.Commands.Setup;
using Wallace.Domain.Entities;

namespace Wallace.Tests.Application
{
    public class SetupValidatorTest : BaseTest
    {
        private SetupValidator _validator;

        [SetUp]
        public void SetUp()
        {
            base.SetUp();
            _validator = new SetupValidator(DbContext);
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
        
        #region AccountName

        [Test]
        public void ShouldFailWithEmptyAccountName()
        {
            _validator.ShouldHaveValidationErrorForEmpty(c => c.AccountName);
        }

        [Test]
        public void ShouldFailWithAccountNamesMoreThan200Characters()
        {
            _validator.ShouldHaveValidationErrorForMoreThanCharacters(
                c => c.AccountName,
                200
            );
        }

        #endregion

        #region BaseCurrency

        [Test]
        public void ShouldFailWithEmptyBaseCurrency()
        {
            _validator.ShouldHaveValidationErrorForEmpty(c => c.BaseCurrency);
        }

        [Test]
        public void ShouldFailWithInvalidCurrencies(
            [Values("EU", "CZX", "A", "Test")]
            string input
        )
        {
            _validator.ShouldHaveValidationErrorFor(
                c => c.BaseCurrency,
                input
            );
        }
        
        [Test]
        public void ShouldNotFailWithValidCurrencies(
            [Values("EUR", "CZK", "USD", "QAR", "RUB")]
            string input
        )
        {
            _validator.ShouldNotHaveValidationErrorFor(
                c => c.BaseCurrency,
                input
            );
        }

        #endregion

        #region InitialBalance

        [Test]
        public void ShouldFailWithLessThan1Balance(
            [Values(-1, -100, 0)]
            int input
        )
        {
            _validator.ShouldHaveValidationErrorFor(
                c => c.InitialBalance,
                input
            );
        }

        [Test]
        public void ShouldAcceptPositiveInitialBalance(
            [Values(1, 100, 1000)] int input
        )
        {
            _validator.ShouldNotHaveValidationErrorFor(
                c => c.InitialBalance,
                input
            );
        }

        #endregion
    }
}