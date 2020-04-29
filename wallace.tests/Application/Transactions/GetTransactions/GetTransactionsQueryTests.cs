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
            SeedTransactionData(TestUserTransaction, OtherTestUserTransaction).Wait();
        }

        [Test]
        public async Task Handle_ShouldReturnAllTransactionsInTheSpecifiedMonthAndYearByLoggedInUser()
        {
            var transactions = new List<Transaction>
            {
                TestUserTransaction,
                OtherTestUserTransaction
            };
            
            var retrievedTransactions = (await _handler.Handle(
                new GetTransactionsQuery
                {
                    Month = TestUserTransaction.Date.Month,
                    Year = TestUserTransaction.Date.Year
                },
                CancellationToken.None
            )).ToList();

            CompareLists(
                transactions,
                retrievedTransactions.Select(ad => Mapper.Map<Transaction>(ad)),
                AssertAreEqual
            );
        }
        
        [Test]
        public async Task Handle_ShouldReturnEmptyListIfNoTransactionsExistInTheGivenMonthAndYear() {
            var retrievedTransactions = (await _handler.Handle(
                new GetTransactionsQuery
                {
                    Month = TestUserTransaction.Date.Month - 1,
                    Year = TestUserTransaction.Date.Year
                },
                CancellationToken.None
            )).ToList();

            Assert.IsEmpty(retrievedTransactions);
            
            retrievedTransactions = (await _handler.Handle(
                new GetTransactionsQuery
                {
                    Month = TestUserTransaction.Date.Month,
                    Year = TestUserTransaction.Date.Year - 1
                },
                CancellationToken.None
            )).ToList();

            Assert.IsEmpty(retrievedTransactions);
            
            retrievedTransactions = (await _handler.Handle(
                new GetTransactionsQuery
                {
                    Month = TestUserTransaction.Date.Month - 1,
                    Year = TestUserTransaction.Date.Year - 1
                },
                CancellationToken.None
            )).ToList();

            Assert.IsEmpty(retrievedTransactions);
        }
    }
}