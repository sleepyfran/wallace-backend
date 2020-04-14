using System.Threading;
using System.Threading.Tasks;
using NodaMoney;
using NUnit.Framework;
using Wallace.Application.Commands.Accounts.EditAccount;
using Wallace.Domain.Exceptions;

namespace Wallace.Tests.Application.Accounts.EditAccount
{
    public class EditAccountCommandTests : BaseTest
    {
        private readonly EditAccountCommand _validInput = new EditAccountCommand
        {
            Id = TestUserAccount.Id,
            QueryId = TestUserAccount.Id,
            Balance = 10,
            Currency = "USD",
            Name = "New Test User Account",
            Owner = TestUser.Id
        };

        private EditAccountCommandHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _handler = new EditAccountCommandHandler(
                DbContext,
                IdentityContainer,
                Mapper
            );

            SeedAccountData(TestUserAccount).Wait();
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldEditAccountForCurrentUserGivenValidInput()
        {
            var editedId = await _handler.Handle(
                _validInput,
                CancellationToken.None
            );

            var account = DbContext.Accounts.Find(editedId);
            
            Assert.IsNotNull(account);
            Assert.AreEqual(_validInput.Name, account.Name);
            Assert.AreEqual(
                new Money(_validInput.Balance, _validInput.Currency),
                account.Balance
            );
            Assert.AreEqual(TestUser.Id, account.Owner.Id);
        }

        [Test]
        public void Handle_ShouldThrowExceptionWhenAccountDoesNotBelongToUser()
        {
            var wrongAccountInput = new EditAccountCommand
            {
                Id = OtherUserAccount.Id,
                QueryId = OtherUserAccount.Id,
                Balance = 10,
                Currency = "USD",
                Name = "Non Matching IDs",
                Owner = TestUser.Id
            };
            
            Assert.ThrowsAsync<AccountNotFoundException>(async () => 
                await _handler.Handle(wrongAccountInput, CancellationToken.None)
            );
        }
    }
}