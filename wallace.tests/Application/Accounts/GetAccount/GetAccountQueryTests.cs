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
        private GetAccountQuery _input;

        private GetAccountQueryHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _handler = new GetAccountQueryHandler(DbContext, IdentityContainer);
            SetIdentityTo(TestUser);
            
            _input = new GetAccountQuery
            {
                Id = TestUserAccount.Id
            };
        }

        [Test]
        public async Task Handle_ShouldReturnAccountIfGuidExists()
        {
            await SeedAccountData(TestUserAccount);

            var queryResult = await _handler.Handle(
                _input,
                CancellationToken.None
            );
            
            AssertEqual(TestUserAccount, Mapper.Map<Account>(queryResult));
        }

        [Test]
        public async Task Handle_ShouldNotReturnAccountsFromOtherUsers()
        {
            await SeedAccountData(OtherUserAccount);

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