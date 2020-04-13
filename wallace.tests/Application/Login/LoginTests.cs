using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Commands.Auth.Login;
using Wallace.Domain.Exceptions;

namespace Wallace.Tests.Application.Login
{
    public class LoginTests : BaseTest
    {
        private LoginCommand _input;

        private LoginCommandHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();

            _handler = new LoginCommandHandler(
                DbContext,
                PasswordHasher,
                TokenBuilder
            );

            _input = new LoginCommand
            {
                Email = TestUser.Email,
                Password = TestUser.Password
            };
        }

        [Test]
        public async Task Handle_ShouldReturnValidTokensGivenValidInput()
        {
            await SeedUserData(TestUser);

            var (accessToken, refreshToken) =
                await _handler.Handle(_input, CancellationToken.None);

            Assert.IsNotNull(accessToken);
            Assert.AreEqual(_input.Email, (string) accessToken);
            Assert.AreEqual((int) TokenLifetime, (int) accessToken.Lifetime);

            Assert.IsNotNull(refreshToken);
            Assert.AreEqual(_input.Email, (string) refreshToken);
            Assert.AreEqual((int) TokenLifetime, (int) refreshToken.Lifetime);
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
            await SeedUserData(TestUser);
            PasswordHasherVerifyResult = false;

            Assert.ThrowsAsync<InvalidCredentialException>(async () =>
                await _handler.Handle(_input, CancellationToken.None)
            );
        }
    }
}