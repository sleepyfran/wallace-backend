using System;
using Microsoft.EntityFrameworkCore;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Entities;

namespace Wallace.Persistence
{
    public class WallaceDbContext : DbContext, IDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Payee> Payees { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public WallaceDbContext(DbContextOptions<WallaceDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(WallaceDbContext).Assembly
            );
        }
    }
}
