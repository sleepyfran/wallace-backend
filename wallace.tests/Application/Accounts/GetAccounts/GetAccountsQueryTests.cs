using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NodaMoney;
using NUnit.Framework;
using Wallace.Application.Queries.Accounts.GetAccounts;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Interfaces;
using Wallace.Domain.Identity.Model;

namespace Wallace.Tests.Application.Accounts.GetAccounts
{
    public class GetAccountsQueryTests : BaseTest
    {
        private IdentityContainer _identityContainer;
        private GetAccountsQueryHandler _handler;
        private User _testUser;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _testUser = new User
            {
                Id = Guid.NewGuid()
            };

            _identityContainer = new IdentityContainer();
            _identityContainer.Set(new UserIdentity { Id = _testUser.Id });
            
            _handler = new GetAccountsQueryHandler(
                DbContext,
                _identityContainer,
                Mapper
            );
        }

        [Test]
        public async Task Handle_ShouldReturnAllAccountsByLoggedInUser()
        {
            var account1 = new Account
            {
                Id = Guid.NewGuid(),
                Balance = Money.Euro(100),
                Name = "Test Account Euro",
                OwnerId = _testUser.Id
            };
            
            var account2 = new Account
            {
                Id = Guid.NewGuid(),
                Balance = Money.USDollar(2000),
                Name = "Test Account Dollars",
                OwnerId = _testUser.Id
            };

            var accounts = new List<Account>
            {
                account1,
                account2
            };

            DbContext.Accounts.AddRange(accounts);

            await DbContext.SaveChangesAsync(CancellationToken.None);

            var retrievedAccounts = (await _handler.Handle(
                new GetAccountsQuery(),
                CancellationToken.None
            )).ToList();

            for (var i = 0; i < retrievedAccounts.Count(); i++)
            {
                var expected = accounts[i];
                var actual = retrievedAccounts[i];
                
                Assert.NotNull(actual);
                Assert.AreEqual(expected.Id, actual.Id);
                Assert.AreEqual(expected.Name, actual.Name);
                Assert.AreEqual(expected.OwnerId, actual.Owner);
                Assert.AreEqual(expected.Balance.Amount, actual.Balance);
                Assert.AreEqual(expected.Balance.Currency.Code, actual.Currency);
            }
;        }

        [Test]
        public async Task Handle_ShouldReturnEmptyListIfNoAccountsExist()
        {
            var retrievedAccounts = await _handler.Handle(
                new GetAccountsQuery(),
                CancellationToken.None
            );
            
            Assert.IsEmpty(retrievedAccounts);
        }
    }
}