using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Commands.Transactions.RemoveTransaction;
using Wallace.Domain.Exceptions;

namespace Wallace.Tests.Application.Transactions.RemoveTransaction
{
    public class RemoveTransactionTests : BaseTest
    {
        private RemoveTransactionCommandHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            _handler = new RemoveTransactionCommandHandler(
                DbContext,
                IdentityContainer,
                Mapper
            );

            SeedTransactionData(TestUserTransaction).Wait();
            SeedTransactionData(OtherTestUserTransaction).Wait();
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldRemoveTransactionGivenValidInput()
        {
            var validInput = new RemoveTransactionCommand
            {
                Id = TestUserTransaction.Id
            };

            var removedId = await _handler.Handle(
                validInput,
                CancellationToken.None
            );

            Assert.IsNotNull(removedId);

            var category = DbContext.Categories.Find(removedId);
            Assert.IsNull(category);
        }

        [Test]
        public void Handle_ShouldThrowExceptionWhenTransactionDoesNotBelongToUser()
        {
            var wrongInput = new RemoveTransactionCommand
            {
                Id = OtherTestUserTransaction.Id
            };

            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _handler.Handle(wrongInput, CancellationToken.None)
            );
        }
    }
}