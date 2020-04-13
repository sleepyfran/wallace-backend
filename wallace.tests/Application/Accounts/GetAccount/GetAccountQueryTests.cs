using System;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using NodaMoney;
using NUnit.Framework;
using Wallace.Application.Queries.Accounts.GetAccount;
using Wallace.Domain.Entities;
using Wallace.Domain.Exceptions;
using Wallace.Domain.Identity.Interfaces;
using Wallace.Domain.Identity.Model;

namespace Wallace.Tests.Application.Accounts.GetAccount
{
    public class GetAccountQueryTests : BaseTest
    {
        private User _testUser = new User
        {
            Id = Guid.NewGuid()
        };
        
        private GetAccountQuery _input = new GetAccountQuery
        {
            Id = Guid.NewGuid()
        };

        private GetAccountQueryHandler _handler;
        private IdentityContainer _identityContainer;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _identityContainer = new IdentityContainer();
            _identityContainer.Set(new UserIdentity { Id = _testUser.Id });
            _handler = new GetAccountQueryHandler(DbContext, _identityContainer);
        }

        [Test]
        public async Task Handle_ShouldReturnAccountIfGuidExists()
        {
            var existingAccount = new Account
            {
                Id = _input.Id,
                Balance = Money.Euro(100),
                Name = "Test Account",
                OwnerId = _testUser.Id
            };

            DbContext.Accounts.Add(existingAccount);
            await DbContext.SaveChangesAsync(CancellationToken.None);

            var queryResult = await _handler.Handle(
                _input,
                CancellationToken.None
            );
            
            Assert.NotNull(queryResult);
            Assert.AreEqual(existingAccount.Id, queryResult.Id);
            Assert.AreEqual(existingAccount.Name, queryResult.Name);
            Assert.AreEqual(existingAccount.OwnerId, queryResult.Owner);
            Assert.AreEqual(existingAccount.Balance.Amount, queryResult.Balance);
            Assert.AreEqual(existingAccount.Balance.Currency.Code, queryResult.Currency);
        }

        [Test]
        public async Task Handle_ShouldNotReturnAccountsFromOtherUsers()
        {
            var accountUser = new User
            {
                Id = Guid.NewGuid()
            };
            
            var account = new Account
            {
                Id = _input.Id,
                Balance = Money.Euro(100),
                Name = "Should not have access to this",
                OwnerId = accountUser.Id
            };

            DbContext.Accounts.Add(account);
            await DbContext.SaveChangesAsync(CancellationToken.None);

            Assert.ThrowsAsync<AccountNotFoundException>(async () => 
                await _handler.Handle(_input, CancellationToken.None)
            );
        }

        [Test]
        public void Handle_ShouldThrowErrorIfGuidDoesNotExists()
        {
            Assert.ThrowsAsync<AccountNotFoundException>(async () => 
                await _handler.Handle(_input, CancellationToken.None)
            );
        }
    }
}