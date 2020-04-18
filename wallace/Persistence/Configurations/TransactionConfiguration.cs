using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallace.Domain.Entities;
using Wallace.Domain.Enums;

namespace Wallace.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder
                .Property(t => t.Type)
                .IsRequired();

            builder
                .Property(t => t.Notes)
                .HasDefaultValue("");

            builder
                .Property(t => t.Repetition)
                .HasDefaultValue(Repetition.Never);

            builder
                .Property(t => t.Amount)
                .HasConversion(Conversions.MoneyToString)
                .IsRequired();

            builder
                .HasOne(a => a.Owner)
                .WithMany(u => u.Transactions)
                .HasForeignKey(a => a.OwnerId);
            
            builder
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId);

            builder
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId);

            builder
                .HasOne(t => t.Payee)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.PayeeId);
        }
    }
}