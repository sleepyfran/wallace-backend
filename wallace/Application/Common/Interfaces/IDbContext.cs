using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wallace.Domain.Entities;

namespace Wallace.Application.Common.Interfaces
{
    /// <summary>
    /// Interface to abstract the use of the database in the application.
    /// </summary>
    public interface IDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Account> Accounts { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Payee> Payees { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
