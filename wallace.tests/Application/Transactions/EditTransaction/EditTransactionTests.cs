using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Commands.Transactions.EditTransaction;
using Wallace.Domain.Entities;
using Wallace.Domain.Enums;
using Wallace.Domain.Exceptions;

namespace Wallace.Tests.Application.Transactions.EditTransaction
{
    public class EditTransactionCommandTests : BaseTest
    {
        private readonly EditTransactionCommand _validInput = new EditTransactionCommand
        {
            Id = TestUserTransaction.Id,
            QueryId = TestUserTransaction.Id,
            Account = TestUserTransaction.AccountId,
            Amount = 1000,
            Category = TestUserTransaction.Category.Id,
            Currency = "USD",
            Date = TestUserTransaction.Date.AddDays(2),
            Notes = "Modified notes",
            Payee = TestUserTransaction.Payee.Id,
            Repetition = Repetition.Weekly,
            Type = TransactionType.Expense,
            Owner = TestUserTransaction.OwnerId
        };

        private EditTransactionCommandHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _handler = new EditTransactionCommandHandler(
                DbContext,
                IdentityContainer,
                Mapper
            );

            SeedTransactionData(TestUserTransaction).Wait();
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldEditTransactionForCurrentUserGivenValidInput()
        {
            var editedId = await _handler.Handle(
                _validInput,
                CancellationToken.None
            );

            var transaction = DbContext.Transactions.Find(editedId);
            
            AssertAreEqual(Mapper.Map<Transaction>(_validInput), transaction);
        }

        [Test]
        public void Handle_ShouldThrowExceptionWhenTransactionDoesNotBelongToUser()
        {
            var wrongTransactionInput = new EditTransactionCommand
            {
                Id = OtherTestUserTransaction.Id,
                QueryId = OtherTestUserTransaction.Id,
                Account = TestUserTransaction.AccountId,
                Amount = TestUserTransaction.Amount.Amount,
                Category = TestUserTransaction.Category.Id,
                Currency = TestUserTransaction.Amount.Currency.Code,
                Date = TestUserTransaction.Date,
                Notes = TestUserTransaction.Notes,
                Payee = TestUserTransaction.Payee.Id,
                Repetition = TestUserTransaction.Repetition,
                Type = TestUserTransaction.Type,
                Owner = OtherTestUserTransaction.OwnerId
            };
            
            Assert.ThrowsAsync<EntityNotFoundException>(async () => 
                await _handler.Handle(wrongTransactionInput, CancellationToken.None)
            );
        }
    }
}