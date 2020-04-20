using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Queries.Categories;
using Wallace.Domain.Entities;

namespace Wallace.Tests.Application.Categories.GetCategories
{
    public class GetCategoriesQueryTests : BaseTest
    {
        private GetCategoriesQueryHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _handler = new GetCategoriesQueryHandler(
                DbContext,
                IdentityContainer,
                Mapper
            );
            
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldReturnAllCategoriesByLoggedInUser()
        {
            await SeedCategoryData(TestUserCategory, OtherTestUserCategory);
            var categories = new List<Category>
            {
                TestUserCategory,
                OtherTestUserCategory
            };
            
            var retrievedCategories = (await _handler.Handle(
                new GetCategoriesQuery(),
                CancellationToken.None
            )).ToList();

            CompareLists(
                categories,
                retrievedCategories.Select(ad => Mapper.Map<Category>(ad)),
                AssertAreEqual
            );
        }
    }
}