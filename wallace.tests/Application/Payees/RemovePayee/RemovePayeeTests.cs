using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Commands.Payees;
using Wallace.Domain.Exceptions;

namespace Wallace.Tests.Application.Categories.RemovePayee
{
    public class RemovePayeeTests : BaseTest
    {
        private RemovePayeeCommandHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            _handler = new RemovePayeeCommandHandler(
                DbContext,
                IdentityContainer,
                Mapper
            );

            SeedPayeeData(TestUserPayee).Wait();
            SeedPayeeData(OtherTestUserPayee).Wait();
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldRemovePayeeGivenValidInput()
        {
            var validInput = new RemovePayeeCommand
            {
                Id = TestUserPayee.Id
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
        public void Handle_ShouldThrowExceptionWhenPayeeDoesNotBelongToUser()
        {
            var wrongInput = new RemovePayeeCommand
            {
                Id = OtherTestUserPayee.Id
            };

            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _handler.Handle(wrongInput, CancellationToken.None)
            );
        }
    }
}