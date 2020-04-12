using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Wallace.Application.Commands.Auth.SignUp;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Tests.Application.SignUp
{
    public class SignUpTests : BaseTest
    {
        private readonly SignUpCommand _validInput = new SignUpCommand
        {
            Email = "test@test.com",
            Name = "Test McTestington",
            Password = "1234"
        };

        private SignUpCommandHandler _handler;

        private string _passwordHasherResult;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();

            _passwordHasherResult = "1234";
            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock
                .Setup(ph => ph.Hash(It.IsAny<string>()))
                .Returns(() => _passwordHasherResult);

            var tokenBuilderMock = new Mock<ITokenBuilder>();
            tokenBuilderMock
                .Setup(tb => tb.BuildAccessToken(It.IsAny<User>()))
                .Returns((User u) => new Token(u.Email) { Lifetime = 10});
            
            tokenBuilderMock
                .Setup(tb => tb.BuildRefreshToken(It.IsAny<User>()))
                .Returns((User u) => new Token(u.Email) { Lifetime = 10});
            
            _handler = new SignUpCommandHandler(
                DbContext,
                passwordHasherMock.Object,
                tokenBuilderMock.Object
            );
        }
        
        [Test]
        public async Task Handle_ShouldPersistValidInput()
        {
            await _handler.Handle(_validInput, CancellationToken.None);

            var user = DbContext.Users.First();
            
            Assert.IsNotNull(user);
            Assert.AreEqual(_validInput.Name, user.Name);
            Assert.AreEqual(_validInput.Email, user.Email);
            Assert.AreEqual(_validInput.Password, user.Password);
        }

        [Test]
        public async Task Handle_ShouldPersistHashedPassword()
        {
            _passwordHasherResult = "hashed";
            await _handler.Handle(_validInput, CancellationToken.None);

            var user = DbContext.Users.First();
            Assert.AreEqual(_passwordHasherResult, user.Password);
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
            Assert.AreEqual(10, (int)accessToken.Lifetime);
            
            Assert.IsNotNull(refreshToken);
            Assert.AreEqual(_validInput.Email, (string)refreshToken);
            Assert.AreEqual(10, (int)refreshToken.Lifetime);
        }
    }
}