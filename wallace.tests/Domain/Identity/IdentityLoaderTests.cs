using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Claims;
using NUnit.Framework;
using Wallace.Domain.Identity;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Tests.Domain.Identity
{
    public class IdentityLoaderTests
    {
        private IdentityContainer _identityContainer;
        private IdentityLoader _identityLoader;
        
        [SetUp]
        public void SetUp()
        {
            _identityContainer = new IdentityContainer();
            _identityLoader = new IdentityLoader(_identityContainer);
        }
        
        [Test]
        public void LoadFrom_ShouldSetIdGivenValidInput()
        {
            _identityLoader.LoadFrom(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "15"),
            });

            var identity = _identityContainer.Get();
            Assert.NotNull(identity);
            Assert.AreEqual(15, identity.Id);
        }

        [Test]
        public void LoadFrom_ShouldThrowExceptionIfNoClaimsProvided()
        {
            Assert.Throws<InvalidCredentialException>(() => 
                _identityLoader.LoadFrom(new List<Claim>())
            );
        }
        
        [Test]
        public void LoadFrom_ShouldThrowExceptionIfNoUserIdClaimProvided()
        {
            Assert.Throws<InvalidCredentialException>(() => 
                _identityLoader.LoadFrom(new []
                {
                    new Claim(ClaimTypes.Actor, "test"),
                })
            );
        }
        
        [Test]
        public void LoadFrom_ShouldThrowExceptionIfUserIdIsNotAnInt()
        {
            Assert.Throws<InvalidCredentialException>(() => 
                _identityLoader.LoadFrom(new []
                {
                    new Claim(ClaimTypes.NameIdentifier, "test"),
                })
            );
        }
    }
}