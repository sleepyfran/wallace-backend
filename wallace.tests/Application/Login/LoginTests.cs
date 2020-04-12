using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Wallace.Application.Commands.Auth.Login;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Entities;
using Wallace.Domain.Exceptions;
using Wallace.Domain.Identity.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Tests.Application.Login
{
    public class LoginTests : BaseTest
    {
        private readonly User _testUser = new User
        {
            Email = "test@test.com",
            Name = "Test McTestington",
            Password = "1234"
        };
        
        private readonly LoginCommand _input = new LoginCommand
        {
            Email = "test@test.com",
            Password = "1234"
        };

        private LoginCommandHandler _handler;

        private bool _passwordHasherResult;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();

            _passwordHasherResult = true;
            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock
                .Setup(ph => ph.Verify(
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ))
                .Returns(() => _passwordHasherResult);

            var tokenBuilderMock = new Mock<ITokenBuilder>();
            tokenBuilderMock
                .Setup(tb => tb.BuildAccessToken(It.IsAny<User>()))
                .Returns((User u) => new Token(u.Email) { Lifetime = 10});
            
            tokenBuilderMock
                .Setup(tb => tb.BuildRefreshToken(It.IsAny<User>()))
                .Returns((User u) => new Token(u.Email) { Lifetime = 10});
            
            _handler = new LoginCommandHandler(
                DbContext,
                passwordHasherMock.Object,
                tokenBuilderMock.Object
            );
        }
        
        [Test]
        public async Task Handle_ShouldReturnValidTokensGivenValidInput()
        {
            DbContext.Users.Add(_testUser);
            await DbContext.SaveChangesAsync(CancellationToken.None);
            
            var (accessToken, refreshToken) = await _handler.Handle(_input, CancellationToken.None);
            
            Assert.IsNotNull(accessToken);
            Assert.AreEqual(_input.Email, (string)accessToken);
            Assert.AreEqual(10, (int)accessToken.Lifetime);
            
            Assert.IsNotNull(refreshToken);
            Assert.AreEqual(_input.Email, (string)refreshToken);
            Assert.AreEqual(10, (int)refreshToken.Lifetime);
        }

        [Test]
        public void Handle_ShouldThrowExceptionGivenNonExistentUser()
        {
            Assert.ThrowsAsync<UserNotFoundException>(async () =>
                await _handler.Handle(_input, CancellationToken.None)
            );
        }

        [Test]
        public async Task Handle_ShouldThrowExceptionGivenInvalidCredentials()
        {
            DbContext.Users.Add(_testUser);
            await DbContext.SaveChangesAsync(CancellationToken.None);
            _passwordHasherResult = false;
            
            Assert.ThrowsAsync<InvalidCredentialException>(async () =>
                await _handler.Handle(_input, CancellationToken.None)
            );
        }
    }
}