using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Commands.Categories.RemoveCategory;
using Wallace.Domain.Exceptions;

namespace Wallace.Tests.Application.Categories.RemoveCategory
{
    public class RemoveCategoryTests : BaseTest
    {
        private RemoveCategoryCommandHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            _handler = new RemoveCategoryCommandHandler(
                DbContext,
                IdentityContainer,
                Mapper
            );

            SeedCategoryData(TestUserCategory).Wait();
            SeedCategoryData(OtherTestUserCategory).Wait();
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldRemoveCategoryGivenValidInput()
        {
            var validInput = new RemoveCategoryCommand
            {
                Id = TestUserCategory.Id
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
        public void Handle_ShouldThrowExceptionWhenCategoryDoesNotBelongToUser()
        {
            var wrongInput = new RemoveCategoryCommand
            {
                Id = OtherTestUserCategory.Id
            };

            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _handler.Handle(wrongInput, CancellationToken.None)
            );
        }
    }
}