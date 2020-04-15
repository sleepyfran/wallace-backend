using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Commands.Accounts.RemoveAccount;
using Wallace.Domain.Exceptions;

namespace Wallace.Tests.Application.Accounts.CreateAccount.RemoveAccount
{
    public class RemoveAccountCommandTests : BaseTest
    {
        private RemoveAccountCommandHandler _handler;
        
        [SetUp]
        public new void SetUp()
        {
            _handler = new RemoveAccountCommandHandler(
                DbContext, 
                IdentityContainer
            );

            SeedAccountData(TestUserAccount).Wait();
            SeedAccountData(OtherUserAccount).Wait();
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldRemoveAccountGivenValidInput()
        {
            var validInput = new RemoveAccountCommand
            {
                Id = TestUserAccount.Id
            };

            var removedId = await _handler.Handle(
                validInput,
                CancellationToken.None
            );

            Assert.IsNotNull(removedId);
            
            var account = DbContext.Accounts.Find(removedId);
            Assert.IsNull(account);
        }
        
        [Test]
        public void Handle_ShouldThrowExceptionWhenAccountDoesNotBelongToUser()
        {
            var wrongAccountInput = new RemoveAccountCommand
            {
                Id = OtherUserAccount.Id
            };
            
            Assert.ThrowsAsync<AccountNotFoundException>(async () => 
                await _handler.Handle(wrongAccountInput, CancellationToken.None)
            );
        }
    }
}