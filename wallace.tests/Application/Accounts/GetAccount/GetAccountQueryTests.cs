using System;
using System.Threading;
using System.Threading.Tasks;
using NodaMoney;
using NUnit.Framework;
using Wallace.Application.Queries.Accounts.GetAccount;
using Wallace.Domain.Entities;
using Wallace.Domain.Exceptions;

namespace Wallace.Tests.Application.Accounts.GetAccount
{
    public class GetAccountQueryTests : BaseTest
    {
        private GetAccountQuery _input = new GetAccountQuery
        {
            Id = Guid.NewGuid()
        };

        private GetAccountQueryHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            _handler = new GetAccountQueryHandler(DbContext);
        }

        [Test]
        public async Task Handle_ShouldReturnAccountIfGuidExists()
        {
            var existingAccount = new Account
            {
                Id = _input.Id,
                Balance = Money.Euro(100),
                Name = "Test Account",
                OwnerId = Guid.NewGuid()
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
        public void Handle_ShouldThrowErrorIfGuidDoesNotExists()
        {
            Assert.ThrowsAsync<AccountNotFoundException>(async () => 
                await _handler.Handle(_input, CancellationToken.None)
            );
        }
    }
}