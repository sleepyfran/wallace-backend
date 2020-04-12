using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Claims;
using NUnit.Framework;
using Wallace.Domain.Identity;
using Wallace.Domain.Identity.Enums;
using Wallace.Domain.Identity.Interfaces;
using static Wallace.Domain.Identity.Model.Instance;

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
            var guid = Guid.NewGuid();
            _identityLoader.LoadFrom(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, guid.ToString()),
            });

            var identity = _identityContainer.Get();
            Assert.NotNull(identity);
            Assert.AreEqual(guid, identity.Id);
            Assert.AreEqual(IdentityType.User, identity.Type);
        }

        [Test]
        public void LoadFrom_ShouldThrowExceptionIfNoClaimsProvided()
        {
            _identityLoader.LoadFrom(new List<Claim>());

            var identity = _identityContainer.Get();
            Assert.AreEqual(Unknown.Id, identity.Id);
            Assert.AreEqual(Unknown.Type, identity.Type);
        }
        
        [Test]
        public void LoadFrom_ShouldThrowExceptionIfNoUserIdClaimProvided()
        {
            _identityLoader.LoadFrom(new []
            {
                new Claim(ClaimTypes.Actor, "test"),
            });

            var identity = _identityContainer.Get();
            Assert.AreEqual(Unknown.Id, identity.Id);
            Assert.AreEqual(Unknown.Type, identity.Type);
        }
        
        [Test]
        public void LoadFrom_ShouldThrowExceptionIfUserIdIsNotAnInt()
        {
            _identityLoader.LoadFrom(new []
            {
                new Claim(ClaimTypes.NameIdentifier, "test"),
            });

            var identity = _identityContainer.Get();
            Assert.AreEqual(Unknown.Id, identity.Id);
            Assert.AreEqual(Unknown.Type, identity.Type);
        }
    }
}