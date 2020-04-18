using System.Threading;
using System.Threading.Tasks;
using NodaMoney;
using NUnit.Framework;
using Wallace.Application.Commands.Accounts.EditAccount;
using Wallace.Domain.Entities;
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
            
            AssertAreEqual(Mapper.Map<Account>(_validInput), account);
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
            
            Assert.ThrowsAsync<EntityNotFoundException>(async () => 
                await _handler.Handle(wrongAccountInput, CancellationToken.None)
            );
        }
    }
}