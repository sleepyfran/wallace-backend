using System;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Commands.Auth.Refresh;

namespace Wallace.Tests.Application.RefreshToken
{
    public class RefreshTokenCommandTests : BaseTest
    {
        private RefreshTokenCommandHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();

            _handler = new RefreshTokenCommandHandler(
                DbContext,
                TokenData,
                TokenChecker,
                TokenBuilder,
                DateTime
            );
        }
        
        [Test]
        public async Task Handle_ShouldRefreshTokenGivenValidInput()
        {
            await SeedUserData(TestUser);
            
            var tokens = await _handler.Handle(
                new RefreshTokenCommand
                {
                    Jwt = TestUser.Email,
                    Expiry = DateTime.UtcNow.AddMinutes(TokenLifetime)
                },
                CancellationToken.None
            );

            Assert.IsNotNull(tokens);
            Assert.AreEqual(TestUser.Email, tokens.AccessToken.Jwt);
            Assert.AreEqual(
                DateTime.UtcNow.AddMinutes(TokenLifetime),
                tokens.AccessToken.Expiry
            );

            Assert.AreEqual(TestUser.Email, tokens.RefreshToken.Jwt);
            Assert.AreEqual(
                DateTime.UtcNow.AddMinutes(TokenLifetime),
                tokens.RefreshToken.Expiry
            );
        }

        [Test]
        public void Handle_ShouldThrowExceptionWhenTokenIsNotValid()
        {
            TokenCheckerResult = false;

            Assert.ThrowsAsync<InvalidCredentialException>(async () =>
                await _handler.Handle(
                    new RefreshTokenCommand(),
                    CancellationToken.None
            ));
        }
        
        [Test]
        public void Handle_ShouldThrowExceptionWhenTokenIdDoesNotExistInToken()
        {
            TokenDataResult = Guid.Empty;

            Assert.ThrowsAsync<InvalidCredentialException>(async () =>
                await _handler.Handle(
                    new RefreshTokenCommand(),
                    CancellationToken.None
                ));
        }
        
        [Test]
        public void Handle_ShouldThrowExceptionWhenUserDoesNotExist()
        {
            Assert.ThrowsAsync<InvalidCredentialException>(async () =>
                await _handler.Handle(
                    new RefreshTokenCommand(),
                    CancellationToken.None
                ));
        }
    }
}