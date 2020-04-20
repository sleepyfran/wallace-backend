using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Commands.Transactions.CreateTransaction;
using Wallace.Domain.Entities;

namespace Wallace.Tests.Application.Transactions.CreateTransaction
{
    public class CreateTransactionCommandTests : BaseTest
    {
        private CreateTransactionCommandHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _handler = new CreateTransactionCommandHandler(
                DbContext,
                IdentityContainer,
                Mapper
            );

            SeedTransactionData(TestUserTransaction).Wait();
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldCreateTransactionForCurrentUserGivenValidInput()
        {
            var validInput = Mapper.Map<CreateTransactionCommand>(
                TestUserTransaction
            );
            
            var transactionId = await _handler.Handle(
                validInput,
                CancellationToken.None
            );

            var transaction = DbContext.Transactions.Find(transactionId);
            
            AssertAreEqual(Mapper.Map<Transaction>(validInput), transaction);
        }

        [Test]
        public async Task Handle_ShouldSetNullableFieldsToNullIfNotProvided()
        {
            var validInput = Mapper.Map<CreateTransactionCommand>(
                TestUserTransaction
            );
            validInput.Payee = Guid.Empty;
            validInput.Category = Guid.Empty;
            
            var transactionId = await _handler.Handle(
                validInput,
                CancellationToken.None
            );

            var transaction = DbContext.Transactions.Find(transactionId);
            
            AssertAreEqual(Mapper.Map<Transaction>(validInput), transaction);
        }
    }
}