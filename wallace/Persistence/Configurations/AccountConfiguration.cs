using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaMoney;
using Wallace.Domain.Entities;

namespace Wallace.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder
                .Property(a => a.Name)
                .IsRequired();

            builder
                .Property(a => a.Balance)
                .HasConversion(Conversions.MoneyToString)
                .IsRequired();

            builder
                .HasOne(a => a.Owner)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.OwnerId);
        }
    }
}