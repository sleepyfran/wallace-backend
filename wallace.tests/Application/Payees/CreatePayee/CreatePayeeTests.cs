using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Commands.Payees;

namespace Wallace.Tests.Application.Payees.CreatePayee
{
    public class CreatePayeeTests : BaseTest
    {
        private CreatePayeeCommandHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _handler = new CreatePayeeCommandHandler(
                DbContext,
                IdentityContainer,
                Mapper
            );

            SeedUserData(TestUser).Wait();
            SetIdentityTo(TestUser);
        }

        [Test]
        public async Task Handle_ShouldCreatePayeeForCurrentUserGivenValidInput()
        {
            var validInput = new CreatePayeeCommand
            {
                Name = TestUserPayee.Name,
                Owner = TestUserPayee.OwnerId
            };
            
            var payeeId = await _handler.Handle(
                validInput,
                CancellationToken.None
            );

            var payee = DbContext.Payees.Find(payeeId);
            
            AssertAreEqual(
                Mapper.Map(validInput, payee),
                payee
            );
        }
    }
}