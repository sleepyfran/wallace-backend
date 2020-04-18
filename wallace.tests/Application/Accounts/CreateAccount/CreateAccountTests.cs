using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Commands.Accounts.CreateAccount;

namespace Wallace.Tests.Application.Accounts.CreateAccount
{
    public class CreateAccountTests : BaseTest
    {
        private readonly CreateAccountCommand _validInput = new CreateAccountCommand
        {
            Name = "Test",
            Balance = 1000,
            Currency = "EUR"
        };
        private CreateAccountCommandHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _handler = new CreateAccountCommandHandler(
                DbContext,
                IdentityContainer,
                Mapper
            );

            SeedUserData(TestUser).Wait();
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldCreateAccountForCurrentUserGivenValidInput()
        {
            var accountId = await _handler.Handle(
                _validInput,
                CancellationToken.None
            );

            var account = DbContext.Accounts.Find(accountId);
            
            AssertAreEqual(
                Mapper.Map(_validInput, account), account
            );
        }
    }
}