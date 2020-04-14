using System;
using System.Linq;
using Wallace.Domain.Entities;
using Wallace.Domain.Exceptions;

namespace Wallace.Domain.Queries
{
    /// <summary>
    /// Methods for querying accounts limiting them to a given user.
    /// </summary>
    public static class UserAccounts
    {
        /// <summary>
        /// Attempts to retrieve an account from an user given its ID and the
        /// account ID to retrieve.
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="accountId"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        /// <exception cref="AccountNotFoundException"></exception>
        public static Account QueryAccountFor(
            this IQueryable<Account> accounts,
            Guid ownerId,
            Guid accountId
        )
        {
            var account = accounts
                .QueryAccountsFor(ownerId)
                .FirstOrDefault(
                    a => a.Id == accountId
                );

            return account ?? throw new AccountNotFoundException();
        }

        /// <summary>
        /// Retrieves all the accounts associated with a given user.
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public static IQueryable<Account> QueryAccountsFor(
            this IQueryable<Account> accounts,
            Guid ownerId
        )
        {
            return accounts
                .Where(a => a.OwnerId == ownerId);
        }
    }
}