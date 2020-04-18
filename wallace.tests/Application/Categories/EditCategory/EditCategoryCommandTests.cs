using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Commands.Categories.EditCategory;
using Wallace.Domain.Entities;
using Wallace.Domain.Exceptions;

namespace Wallace.Tests.Application.Categories.EditCategories
{
    public class EditCategoryCommandTests : BaseTest
    {
        private readonly EditCategoryCommand _validInput = new EditCategoryCommand
        {
            Id = TestUserCategory.Id,
            QueryId = TestUserCategory.Id,
            Emoji = "🔥",
            Name = "New Test User Account",
            Owner = TestUser.Id
        };

        private EditCategoryCommandHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _handler = new EditCategoryCommandHandler(
                DbContext,
                IdentityContainer,
                Mapper
            );

            SeedCategoryData(TestUserCategory).Wait();
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldEditCategoryForCurrentUserGivenValidInput()
        {
            var editedId = await _handler.Handle(
                _validInput,
                CancellationToken.None
            );

            var category = DbContext.Categories.Find(editedId);
            
            AssertAreEqual(Mapper.Map<Category>(_validInput), category);
        }

        [Test]
        public void Handle_ShouldThrowExceptionWhenCategoryDoesNotBelongToUser()
        {
            var wrongCategoryInput = new EditCategoryCommand
            {
                Id = OtherUserCategory.Id,
                QueryId = OtherUserCategory.Id,
                Emoji = OtherUserCategory.Emoji,
                Name = "Non Matching IDs",
                Owner = OtherTestUser.Id
            };
            
            Assert.ThrowsAsync<EntityNotFoundException>(async () => 
                await _handler.Handle(wrongCategoryInput, CancellationToken.None)
            );
        }
    }
}