using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using NodaMoney;
using NUnit.Framework;
using Wallace.Application.Common.Interfaces;
using Wallace.Application.Common.Mappings;
using Wallace.Domain.Common.Interfaces;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Entities;
using Wallace.Domain.Identity.Interfaces;
using Wallace.Domain.Identity.Model;
using Wallace.Domain.ValueObjects;
using Wallace.Persistence;

namespace Wallace.Tests
{
    public class BaseTest
    {
        protected IDbContext DbContext;
        protected IdentityContainer IdentityContainer;
        protected IMapper Mapper;
        protected IDateTime DateTime;
        protected IPasswordHasher PasswordHasher;
        protected ITokenBuilder TokenBuilder;
        protected ITokenChecker TokenChecker;
        protected ITokenData TokenData;
        
        protected bool ReloadOnSetUp = true;
        protected DateTime DateTimeResult = System.DateTime.Now;
        protected bool PasswordHasherVerifyResult = true;
        protected string PasswordHasherResult = "hashed";
        protected Minutes TokenLifetime = 10;
        protected bool TokenCheckerResult = true;
        protected Guid TokenDataResult = TestUser.Id;

        #region Testing data

        protected static readonly User TestUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "testuser@email.com",
            Name = "Test User",
            Password = "1234"
        };
            
        protected static readonly User OtherTestUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "otheruser@email.com",
            Name = "Other User",
            Password = "1234"
        };
            
        protected static readonly Account TestUserAccount = new Account
        {
            Id = Guid.NewGuid(),
            Balance = Money.Euro(100),
            Name = "Test User Account",
            OwnerId = TestUser.Id
        };
            
        protected static readonly Account AnotherTestUserAccount = new Account
        {
            Id = Guid.NewGuid(),
            Balance = Money.Euro(1000),
            Name = "Another Test User Account",
            OwnerId = TestUser.Id
        };
            
        protected static readonly Account OtherUserAccount = new Account
        {
            Id = Guid.NewGuid(),
            Balance = Money.Euro(100),
            Name = "Other User Account",
            OwnerId = OtherTestUser.Id
        };

        #endregion
        
        [SetUp]
        public void SetUp()
        {
            if (ReloadOnSetUp)
                CreateContext();

            SetupMocksAndDependencies();
        }
        
        private void CreateContext()
        {
            var options = new DbContextOptionsBuilder<WallaceDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            
            DbContext = new WallaceDbContext(options);
        }

        private void SetupMocksAndDependencies()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            
            var dateTimeMock = new Mock<IDateTime>();
            dateTimeMock
                .Setup(dt => dt.UtcNow)
                .Returns(() => DateTimeResult);

            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock
                .Setup(ph => ph.Hash(It.IsAny<string>()))
                .Returns(() => PasswordHasherResult);

            passwordHasherMock
                .Setup(ph => ph.Verify(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => PasswordHasherVerifyResult);
            
            var tokenBuilderMock = new Mock<ITokenBuilder>();
            tokenBuilderMock
                .Setup(tb => tb.BuildAccessToken(It.IsAny<User>()))
                .Returns((User u) => new Token(u.Email) { Lifetime = TokenLifetime});
            
            tokenBuilderMock
                .Setup(tb => tb.BuildRefreshToken(It.IsAny<User>()))
                .Returns((User u) => new Token(u.Email) { Lifetime = TokenLifetime});
            
            var tokenDataMock = new Mock<ITokenData>();
            tokenDataMock
                .Setup(td => td.GetUserIdFrom(It.IsAny<Token>()))
                .Returns(() => TokenDataResult);

            var tokenCheckerMock = new Mock<ITokenChecker>();
            tokenCheckerMock
                .Setup(tc => tc.IsValid(
                        It.IsAny<Token>(),
                        It.IsAny<DateTime>()
                    )
                )
                .Returns(() => TokenCheckerResult);

            IdentityContainer = new IdentityContainer();
            Mapper = configuration.CreateMapper();
            DateTime = dateTimeMock.Object;
            PasswordHasher = passwordHasherMock.Object;
            TokenBuilder = tokenBuilderMock.Object;
            TokenData = tokenDataMock.Object;
            TokenChecker = tokenCheckerMock.Object;
        }

        /// <summary>
        /// Sets the identity in the container to the ID of the given user.
        /// </summary>
        /// <param name="user"></param>
        protected void SetIdentityTo(User user)
        {
            IdentityContainer.Set(new UserIdentity
            {
                Id = user.Id
            });
        }

        /// <summary>
        /// Sets the identity in the container to the unknown identity.
        /// </summary>
        /// <param name="user"></param>
        protected void SetIdentityToUnknown()
        {
            IdentityContainer.Set(new Unknown());
        }

        /// <summary>
        /// Adds the given users to the test database data.
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        protected async Task SeedUserData(params User[] users)
        {
            DbContext.Users.AddRange(users.Select(Clone));
            await DbContext.SaveChangesAsync(CancellationToken.None);
        }

        /// <summary>
        /// Adds the given accounts to the test database data. If the users
        /// linked to the accounts are not present, it'll try to add them
        /// with the matching test user, if none it's found then it'll throw
        /// an exception since an account cannot exist without an user.
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        protected async Task SeedAccountData(params Account[] accounts)
        {
            await EnsureUsersAdded(accounts.Select(a => a.OwnerId));
            DbContext.Accounts.AddRange(accounts.Select(Clone));
            await DbContext.SaveChangesAsync(CancellationToken.None);
        }
        
        /// <summary>
        /// Returns a cloned copy of the object. Useful when inserting test
        /// data into the database so the original reference will never be
        /// modified.
        /// </summary>
        private static T Clone<T>(T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
        
        /// <summary>
        /// Checks that the given user IDs are present in the database and
        /// attempts to match them with the test data and add them if not.
        /// </summary>
        private async Task EnsureUsersAdded(IEnumerable<Guid> userIds)
        {
            var nonPresentIds = userIds
                .Where(id => !DbContext.Users.Any(u => u.Id == id))
                .Distinct()
                .ToList();

            if (!nonPresentIds.Any())
                return;

            var availableUsers = new List<User>
            {
                TestUser,
                OtherTestUser
            };
            foreach (var id in nonPresentIds)
            {
                var user = availableUsers.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    throw new ConstraintException(
                        $"The user with ID {id} was linked to an account but is not present in the database and could not be matched with any test user"
                    );
                }

                DbContext.Users.Add(Clone(user));
            }

            await DbContext.SaveChangesAsync(CancellationToken.None);
        }

        protected void AssertEqual(User expected, User actual)
        {
            Assert.NotNull(expected);
            Assert.NotNull(actual);
            
            if (expected.Id != Guid.Empty && actual.Id != Guid.Empty)
                Assert.AreEqual(expected.Id, actual.Id);
            
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Password, actual.Password);
        }
        
        protected void AssertEqual(IEnumerable<User> expected, IEnumerable<User> actual)
        {
            var expectedList = expected.ToList();
            var actualList = actual.ToList();
            
            for (var i = 0; i < actualList.Count(); i++)
            {
                AssertEqual(expectedList[i], actualList[i]);
            }
        }

        protected void AssertEqual(Account expected, Account actual)
        {
            Assert.NotNull(expected);
            Assert.NotNull(actual);
            
            if (expected.Id != Guid.Empty && actual.Id != Guid.Empty)
                Assert.AreEqual(expected.Id, actual.Id);
            
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Balance, actual.Balance);
            Assert.AreEqual(expected.OwnerId, actual.OwnerId);
        }

        protected void AssertEqual(IEnumerable<Account> expected, IEnumerable<Account> actual)
        {
            var expectedList = expected.ToList();
            var actualList = actual.ToList();
            
            for (var i = 0; i < actualList.Count(); i++)
            {
                AssertEqual(expectedList[i], actualList[i]);
            }
        }
    }
}