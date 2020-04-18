using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallace.Domain.Entities;

namespace Wallace.Persistence.Configurations
{
    public class PayeeConfiguration : IEntityTypeConfiguration<Payee>
    {
        public void Configure(EntityTypeBuilder<Payee> builder)
        {
            builder
                .Property(p => p.Name)
                .IsRequired();

            builder
                .HasMany(p => p.Transactions)
                .WithOne(t => t.Payee)
                .HasForeignKey(t => t.PayeeId);
            
            builder
                .HasOne(a => a.Owner)
                .WithMany(u => u.Payees)
                .HasForeignKey(a => a.OwnerId);
        }
    }
}