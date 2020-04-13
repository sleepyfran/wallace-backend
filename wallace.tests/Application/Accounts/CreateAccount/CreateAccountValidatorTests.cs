using FluentValidation.TestHelper;
using NUnit.Framework;
using Wallace.Application.Commands.Accounts.CreateAccount;

namespace Wallace.Tests.Application.Accounts.CreateAccount
{
    public class CreateAccountValidatorTests
    {
        private CreateAccountValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateAccountValidator();
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

        #region Currency

        [Test]
        public void ShouldFailWithEmptyCurrency()
        {
            _validator.ShouldHaveValidationErrorForEmpty(c => c.Currency);
        }

        [Test]
        public void ShouldFailWithInvalidCurrencies(
            [Values("EU", "CZX", "A", "Test")]
            string input
        )
        {
            _validator.ShouldHaveValidationErrorFor(
                c => c.Currency,
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
                c => c.Currency,
                input
            );
        }

        #endregion

        #region Balance

        [Test]
        public void ShouldFailWithLessThan1Balance(
            [Values(-1, -100, 0)]
            int input
        )
        {
            _validator.ShouldHaveValidationErrorFor(
                c => c.Balance,
                input
            );
        }

        [Test]
        public void ShouldAcceptPositiveBalance(
            [Values(1, 100, 1000)] int input
        )
        {
            _validator.ShouldNotHaveValidationErrorFor(
                c => c.Balance,
                input
            );
        }

        #endregion
    }
}