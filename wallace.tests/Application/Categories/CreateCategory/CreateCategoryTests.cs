using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Commands.Categories.CreateCategory;

namespace Wallace.Tests.Application.Categories.CreateCategory
{
    public class CreateCategoryTests : BaseTest
    {
        private CreateCategoryCommandHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _handler = new CreateCategoryCommandHandler(
                DbContext,
                IdentityContainer,
                Mapper
            );

            SeedUserData(TestUser).Wait();
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldCreateCategoryForCurrentUserGivenValidInput()
        {
            var validInput = new CreateCategoryCommand
            {
                Name = "Test Category",
                Emoji = "🔥"
            };
            
            var categoryId = await _handler.Handle(
                validInput,
                CancellationToken.None
            );

            var category = DbContext.Categories.Find(categoryId);
            
            AssertAreEqual(
                Mapper.Map(validInput, category),
                category
            );
        }
    }
}