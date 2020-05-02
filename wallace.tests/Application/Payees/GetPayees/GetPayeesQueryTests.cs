using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Queries.Payees;
using Wallace.Domain.Entities;

namespace Wallace.Tests.Application.Payees.GetPayees
{
    public class GetPayeesQueryTests : BaseTest
    {
        private GetPayeesQueryHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _handler = new GetPayeesQueryHandler(
                DbContext,
                IdentityContainer,
                Mapper
            );
            
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldReturnAllPayeesByLoggedInUser()
        {
            await SeedPayeeData(TestUserPayee, OtherTestUserPayee);
            var payees = new List<Payee>
            {
                TestUserPayee,
                OtherTestUserPayee
            };
            
            var retrievedPayees = (await _handler.Handle(
                new GetPayeesQuery(),
                CancellationToken.None
            )).ToList();

            CompareLists(
                payees,
                retrievedPayees.Select(ad => Mapper.Map<Payee>(ad)),
                AssertAreEqual
            );
        }
    }
}