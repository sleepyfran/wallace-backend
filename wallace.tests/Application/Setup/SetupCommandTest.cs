using System.Threading;
using System.Threading.Tasks;
using Moq;
using NodaMoney;
using NUnit.Framework;
using Wallace.Application.Commands.Setup;
using Wallace.Application.Common.Interfaces;

namespace Wallace.Tests.Application
{
    public class SetupCommandTest : BaseTest
    {
        private IPasswordHasher _passwordHasher;
        
        [SetUp]
        public void SetUp()
        {
            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock
                .Setup(ph => ph.Hash(It.IsAny<string>()))
                .Returns((string i) => i);
            
            _passwordHasher = passwordHasherMock.Object;
        }
        
        [Test]
        public async Task Handle_ShouldPersistValidInput()
        {
            var command = new SetupCommand
            {
                Email = "testemail@test.com",
                Name = "Test",
                Password = "1234",
                AccountName = "Test Account",
                BaseCurrency = "EUR",
                CategorySelection = CategorySelection.Default,
                InitialBalance = 1000
            };
            
            var handler = new SetupCommandHandler(DbContext, _passwordHasher);
            var (userId, accountId) = await handler.Handle(command, CancellationToken.None);

            var user = DbContext.Users.Find(userId);
            var account = DbContext.Accounts.Find(accountId);
            
            Assert.IsNotNull(user);
            Assert.AreEqual("Test", user.Name);
            Assert.AreEqual("testemail@test.com", user.Email);
            Assert.AreEqual("1234", user.Password);
            
            Assert.IsNotNull(account);
            Assert.AreEqual("Test Account", account.Name);
            Assert.AreEqual(Money.Euro(1000), account.Balance);
        }
    }
}