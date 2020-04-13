using System;
using System.Collections.Generic;
using System.Security.Claims;
using NUnit.Framework;
using Wallace.Domain.Identity;
using Wallace.Domain.Identity.Enums;
using static Wallace.Domain.Identity.Model.Instance;

namespace Wallace.Tests.Domain.Identity
{
    public class IdentityLoaderTests : BaseTest
    {
        private IdentityLoader _sut;
        
        [SetUp]
        public new void SetUp()
        {
            _sut = new IdentityLoader(IdentityContainer);
        }
        
        [Test]
        public void LoadFrom_ShouldSetIdGivenValidInput()
        {
            var guid = Guid.NewGuid();
            _sut.LoadFrom(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, guid.ToString()),
            });

            var identity = IdentityContainer.Get();
            Assert.NotNull(identity);
            Assert.AreEqual(guid, identity.Id);
            Assert.AreEqual(IdentityType.User, identity.Type);
        }

        [Test]
        public void LoadFrom_ShouldThrowExceptionIfNoClaimsProvided()
        {
            _sut.LoadFrom(new List<Claim>());

            var identity = IdentityContainer.Get();
            Assert.AreEqual(Unknown.Id, identity.Id);
            Assert.AreEqual(Unknown.Type, identity.Type);
        }
        
        [Test]
        public void LoadFrom_ShouldThrowExceptionIfNoUserIdClaimProvided()
        {
            _sut.LoadFrom(new []
            {
                new Claim(ClaimTypes.Actor, "test"),
            });

            var identity = IdentityContainer.Get();
            Assert.AreEqual(Unknown.Id, identity.Id);
            Assert.AreEqual(Unknown.Type, identity.Type);
        }
        
        [Test]
        public void LoadFrom_ShouldThrowExceptionIfUserIdIsNotAnInt()
        {
            _sut.LoadFrom(new []
            {
                new Claim(ClaimTypes.NameIdentifier, "test"),
            });

            var identity = IdentityContainer.Get();
            Assert.AreEqual(Unknown.Id, identity.Id);
            Assert.AreEqual(Unknown.Type, identity.Type);
        }
    }
}