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
            
            var refreshToken = await _handler.Handle(
                new RefreshTokenCommand(),
                CancellationToken.None
            );
            
            Assert.IsNotNull(refreshToken);
            Assert.AreEqual(TestUser.Email, (string)refreshToken);
            Assert.AreEqual(10, (int)refreshToken.Lifetime);
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