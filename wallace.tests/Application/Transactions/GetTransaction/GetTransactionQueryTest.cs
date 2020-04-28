using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Queries.Transactions;
using Wallace.Domain.Entities;
using Wallace.Domain.Exceptions;

namespace Wallace.Tests.Application.Transactions.GetTransaction
{
    public class GetTransactionQueryTests : BaseTest
    {
        private GetTransactionQuery _input;

        private GetTransactionQueryHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _handler = new GetTransactionQueryHandler(
                DbContext, 
                IdentityContainer,
                Mapper
            );
            SetIdentityTo(TestUser);
            
            _input = new GetTransactionQuery
            {
                Id = TestUserTransaction.Id
            };
        }

        [Test]
        public async Task Handle_ShouldReturnTransactionIfGuidExists()
        {
            await SeedTransactionData(TestUserTransaction);

            var queryResult = await _handler.Handle(
                _input,
                CancellationToken.None
            );
            
            AssertAreEqual(TestUserTransaction, Mapper.Map<Transaction>(queryResult));
        }

        [Test]
        public async Task Handle_ShouldNotReturnTransactionsFromOtherUsers()
        {
            await SeedTransactionData(OtherTestUserTransaction);

            Assert.ThrowsAsync<EntityNotFoundException>(async () => 
                await _handler.Handle(_input, CancellationToken.None)
            );
        }

        [Test]
        public void Handle_ShouldThrowErrorIfGuidDoesNotExists()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => 
                await _handler.Handle(_input, CancellationToken.None)
            );
        }
    }
}