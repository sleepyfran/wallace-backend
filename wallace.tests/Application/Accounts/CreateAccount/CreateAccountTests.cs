using System.Threading;
using System.Threading.Tasks;
using NodaMoney;
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
                IdentityContainer
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
            
            Assert.IsNotNull(account);
            Assert.AreEqual("Test", account.Name);
            Assert.AreEqual(Money.Euro(1000), account.Balance);
            Assert.AreEqual(TestUser.Id, account.Owner.Id);
        }
    }
}