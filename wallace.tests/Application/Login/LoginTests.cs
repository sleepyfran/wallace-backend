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
                TokenBuilder,
                DateTime
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

            var auth = await _handler.Handle(_input, CancellationToken.None);

            Assert.IsNotNull(auth);

            Assert.IsNotNull(auth.Token);
            Assert.AreEqual(TestUser.Email, auth.Token.AccessToken.Jwt);
            Assert.AreEqual(
                DateTime.UtcNow.AddMinutes(TokenLifetime),
                auth.Token.AccessToken.Expiry
            );

            Assert.AreEqual(TestUser.Email, auth.Token.RefreshToken.Jwt);
            Assert.AreEqual(
                DateTime.UtcNow.AddMinutes(TokenLifetime),
                auth.Token.RefreshToken.Expiry
            );

            Assert.AreEqual(TestUser.Id, auth.Id);
            Assert.AreEqual(TestUser.Email, auth.Email);
            Assert.AreEqual(TestUser.Name, auth.Name);
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