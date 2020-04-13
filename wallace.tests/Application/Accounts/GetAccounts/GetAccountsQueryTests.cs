using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Queries.Accounts.GetAccounts;
using Wallace.Domain.Entities;

namespace Wallace.Tests.Application.Accounts.GetAccounts
{
    public class GetAccountsQueryTests : BaseTest
    {
        private GetAccountsQueryHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _handler = new GetAccountsQueryHandler(
                DbContext,
                IdentityContainer,
                Mapper
            );
            
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldReturnAllAccountsByLoggedInUser()
        {
            await SeedAccountData(TestUserAccount, AnotherTestUserAccount);
            var accounts = new List<Account>
            {
                TestUserAccount,
                AnotherTestUserAccount
            };
            
            var retrievedAccounts = (await _handler.Handle(
                new GetAccountsQuery(),
                CancellationToken.None
            )).ToList();

            AssertEqual(accounts, retrievedAccounts.Select(ad => Mapper.Map<Account>(ad)))
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