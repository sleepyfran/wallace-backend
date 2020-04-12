using System;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Wallace.Application.Commands.Auth.Refresh;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Entities;
using Wallace.Domain.Identity.Interfaces;
using Wallace.Tests.Mocks;

namespace Wallace.Tests.Application.RefreshToken
{
    public class RefreshTokenCommandTests : BaseTest
    {
        private User _testUser = new User
        {
            UserId = 1,
            Email = "test@test.com"
        };
        private DateTime _now;
        private RefreshTokenCommandHandler _handler;
        private bool _tokenCheckerResult;
        private int? _tokenDataResult;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            
            _now = DateTime.Now;
            _tokenCheckerResult = true;
            _tokenDataResult = 1;

            var tokenDataMock = new Mock<ITokenData>();
            tokenDataMock
                .Setup(td => td.GetUserIdFrom(It.IsAny<Token>()))
                .Returns(() => _tokenDataResult);

            var tokenCheckerMock = new Mock<ITokenChecker>();
            tokenCheckerMock
                .Setup(tc => tc.IsValid(
                        It.IsAny<Token>(),
                        It.IsAny<DateTime>()
                    )
                )
                .Returns(() => _tokenCheckerResult);

            var tokenBuilderMock = new Mock<ITokenBuilder>();
            tokenBuilderMock
                .Setup(tb => tb.BuildAccessToken(It.IsAny<User>()))
                .Returns((User user) => new Token(user.Email)
                {
                    Lifetime = 10
                });
            
            _handler = new RefreshTokenCommandHandler(
                DbContext,
                tokenDataMock.Object,
                tokenCheckerMock.Object,
                tokenBuilderMock.Object,
                new MockDateTime(_now)
            );
        }
        
        [Test]
        public async Task Handle_ShouldRefreshTokenGivenValidInput()
        {
            DbContext.Users.Add(_testUser);
            await DbContext.SaveChangesAsync(CancellationToken.None);
            
            var refreshToken = await _handler.Handle(
                new RefreshTokenCommand(),
                CancellationToken.None
            );
            
            Assert.IsNotNull(refreshToken);
            Assert.AreEqual(_testUser.Email, (string)refreshToken);
            Assert.AreEqual(10, (int)refreshToken.Lifetime);
        }

        [Test]
        public void Handle_ShouldThrowExceptionWhenTokenIsNotValid()
        {
            _tokenCheckerResult = false;

            Assert.ThrowsAsync<InvalidCredentialException>(async () =>
                await _handler.Handle(
                    new RefreshTokenCommand(),
                    CancellationToken.None
            ));
        }
        
        [Test]
        public void Handle_ShouldThrowExceptionWhenTokenIdDoesNotExistInToken()
        {
            _tokenDataResult = null;

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