using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Commands.Auth.SignUp;
using Wallace.Domain.Entities;

namespace Wallace.Tests.Application.SignUp
{
    public class SignUpTests : BaseTest
    {
        private SignUpCommand _validInput;

        private SignUpCommandHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();

            _handler = new SignUpCommandHandler(
                DbContext,
                PasswordHasher,
                TokenBuilder
            );
            
            _validInput = new SignUpCommand
            {
                Email = TestUser.Email,
                Name = TestUser.Name,
                Password = TestUser.Password
            };
        }
        
        [Test]
        public async Task Handle_ShouldPersistValidInput()
        {
            await _handler.Handle(_validInput, CancellationToken.None);

            var user = DbContext.Users.First();
            
            AssertAreEqual(new User
            {
                Email = _validInput.Email,
                Name = _validInput.Name,
                Password = PasswordHasherResult
            }, user);
        }

        [Test]
        public async Task Handle_ShouldPersistHashedPassword()
        {
            await _handler.Handle(_validInput, CancellationToken.None);

            var user = DbContext.Users.First();
            Assert.AreEqual(PasswordHasherResult, user.Password);
        }

        [Test]
        public async Task Handle_ShouldCreateValidToken()
        {
            var (accessToken, refreshToken) = await _handler.Handle(
                _validInput,
                CancellationToken.None
            );
            
            Assert.IsNotNull(accessToken);
            Assert.AreEqual(_validInput.Email, (string)accessToken);
            Assert.AreEqual((int)TokenLifetime, (int)accessToken.Lifetime);
            
            Assert.IsNotNull(refreshToken);
            Assert.AreEqual(_validInput.Email, (string)refreshToken);
            Assert.AreEqual((int)TokenLifetime, (int)refreshToken.Lifetime);
        }
    }
}