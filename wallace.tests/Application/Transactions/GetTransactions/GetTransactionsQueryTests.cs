using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Queries.Transactions;
using Wallace.Domain.Entities;

namespace Wallace.Tests.Application.Transactions.GetTransactions
{
    public class GetTransactionsQueryTests : BaseTest
    {
        private GetTransactionsQueryHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _handler = new GetTransactionsQueryHandler(
                DbContext,
                IdentityContainer,
                Mapper
            );
            
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldReturnAllTransactionsByLoggedInUser()
        {
            await SeedTransactionData(TestUserTransaction, OtherTestUserTransaction);
            var transactions = new List<Transaction>
            {
                TestUserTransaction,
                OtherTestUserTransaction
            };
            
            var retrievedTransactions = (await _handler.Handle(
                new GetTransactionsQuery(),
                CancellationToken.None
            )).ToList();

            CompareLists(
                transactions,
                retrievedTransactions.Select(ad => Mapper.Map<Transaction>(ad)),
                AssertAreEqual
            );
        }
    }
}