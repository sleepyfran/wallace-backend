using System;
using System.Linq;
using NUnit.Framework;
using Wallace.Domain.Exceptions;
using Wallace.Domain.Queries;

namespace Wallace.Tests.Domain.QueryableExtensions
{
    public class Account : BaseTest
    {
        [SetUp]
        public new void SetUp()
        {
            base.SetUp();

            SeedAccountData(TestUserAccount).Wait();
        }

        #region QueryAccountFor

        [Test]
        public void QueryAccountsFor_ShouldReturnAccountIfExists()
        {
            AssertEqual(
                TestUserAccount,
                DbContext.Accounts
                    .QueryAccountFor(
                        TestUser.Id,
                        TestUserAccount.Id
                    )
            );
        }
        
        [Test]
        public void QueryAccountsFor_ShouldThrowErrorIfAccountDoesNotExist()
        {
            Assert.Throws<AccountNotFoundException>(() => 
                DbContext.Accounts.QueryAccountFor(
                    TestUser.Id, 
                    Guid.NewGuid()
                )
            );
        }
        
        [Test]
        public void QueryAccountsFor_ShouldThrowErrorIfAccountDoesNotBelongToUser()
        {
            Assert.Throws<AccountNotFoundException>(() => 
                DbContext.Accounts.QueryAccountFor(
                    TestUser.Id, 
                    OtherUserAccount.Id
                )
            );
        }

        #endregion

        #region QueryAccountsFor
        
        [Test]
        public void QueryAccountsFor_ShouldReturnEmptyListIfNoAccountsExist()
        {
            var accounts = DbContext.Accounts
                .QueryAccountsFor(OtherTestUser.Id);
            
            Assert.IsEmpty(accounts);
        }

        [Test]
        public void QueryAccountsFor_ShouldReturnOnlyAccountsOfGivenUser()
        {
            SeedAccountData(OtherUserAccount).Wait();

            var accounts = DbContext.Accounts
                .QueryAccountsFor(OtherTestUser.Id);
            
            Assert.AreEqual(1, accounts.Count());
            AssertEqual(OtherUserAccount, accounts.First());
        }

        #endregion
    }
}