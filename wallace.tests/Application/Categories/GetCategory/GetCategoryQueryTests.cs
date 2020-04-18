using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Queries.Categories;
using Wallace.Domain.Entities;
using Wallace.Domain.Exceptions;

namespace Wallace.Tests.Application.Categories.GetCategory
{
    public class GetCategoryQueryTests : BaseTest
    {
        private GetCategoryQuery _input;

        private GetCategoryQueryHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _handler = new GetCategoryQueryHandler(
                DbContext, 
                IdentityContainer,
                Mapper
            );
            SetIdentityTo(TestUser);
            
            _input = new GetCategoryQuery
            {
                Id = TestUserCategory.Id
            };
        }

        [Test]
        public async Task Handle_ShouldReturnCategoryIfGuidExists()
        {
            await SeedCategoryData(TestUserCategory);

            var queryResult = await _handler.Handle(
                _input,
                CancellationToken.None
            );
            
            AssertAreEqual(TestUserCategory, Mapper.Map<Category>(queryResult));
        }

        [Test]
        public async Task Handle_ShouldNotReturnCategoriesFromOtherUsers()
        {
            await SeedCategoryData(OtherUserCategory);

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