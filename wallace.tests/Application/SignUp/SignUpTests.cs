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
        public async Task Handle_ShouldReturnValidDetails()
        {
            var auth = await _handler.Handle(
                _validInput,
                CancellationToken.None
            );

            Assert.IsNotNull(auth);

            Assert.IsNotNull(auth.Token);
            Assert.AreEqual(_validInput.Email, (string)auth.Token.AccessToken);
            Assert.AreEqual((int)TokenLifetime, (int)auth.Token.AccessToken.Lifetime);
            Assert.AreEqual(_validInput.Email, (string)auth.Token.RefreshToken);
            Assert.AreEqual((int)TokenLifetime, (int)auth.Token.RefreshToken.Lifetime);

            Assert.AreEqual(_validInput.Email, auth.Email);
            Assert.AreEqual(_validInput.Name, auth.Name);
        }
    }
}