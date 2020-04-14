using System;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Wallace.Application.Commands.Accounts.EditAccount;

namespace Wallace.Tests.Application.Accounts.CreateAccount.EditAccount
{
    public class EditAccountValidatorTests
    {
        private EditAccountValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new EditAccountValidator();
        }

        [Test]
        public void ShouldNotFailIfIdsMatch()
        {
            var id = Guid.NewGuid();
            var result = _validator.TestValidate(new EditAccountCommand
            {
                Id = id,
                QueryId = id,
                Balance = 10,
                Currency = "USD",
                Name = "Non Matching IDs",
                Owner = Guid.NewGuid()
            });
            
            result.ShouldNotHaveAnyValidationErrors();
        }
        
        [Test]
        public void ShouldFailWhenIdsDoNotMatch()
        {
            var result = _validator.TestValidate(new EditAccountCommand
            {
                Id = Guid.NewGuid(),
                QueryId = Guid.NewGuid(),
                Balance = 10,
                Currency = "USD",
                Name = "Non Matching IDs",
                Owner = Guid.NewGuid()
            });

            result.ShouldHaveValidationErrorFor(
                c => c.QueryId
            );
        }
    }
}