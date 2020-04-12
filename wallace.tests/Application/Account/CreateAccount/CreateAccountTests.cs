using System.Threading;
using System.Threading.Tasks;
using NodaMoney;
using NUnit.Framework;
using Wallace.Application.Commands.Accounts.CreateAccount;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Interfaces;
using Wallace.Domain.Identity.Model;

namespace Wallace.Tests.Application.Account.CreateAccount
{
    public class CreateAccountTests : BaseTest
    {
        private CreateAccountCommand _validInput = new CreateAccountCommand
        {
            Name = "Test",
            Balance = 1000,
            Currency = "EUR"
        };
        private CreateAccountCommandHandler _handler;
        private IdentityContainer _identityContainer;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            _identityContainer = new IdentityContainer();
            _handler = new CreateAccountCommandHandler(
                DbContext,
                _identityContainer
            );
        }

        [Test]
        public async Task Handle_ShouldCreateAccountForCurrentUserGivenValidInput()
        {
            var owner = new User
            {
                UserId = 1
            };
            DbContext.Users.Add(owner);
            await DbContext.SaveChangesAsync(CancellationToken.None);
            
            _identityContainer.Set(new UserIdentity { Id = owner.UserId });
            
            var accountId = await _handler.Handle(
                _validInput,
                CancellationToken.None
            );

            var account = DbContext.Accounts.Find(accountId);
            
            Assert.IsNotNull(account);
            Assert.AreEqual("Test", account.Name);
            Assert.AreEqual(Money.Euro(1000), account.Balance);
            Assert.AreEqual(owner.UserId, account.Owner.UserId);
        }
    }
}