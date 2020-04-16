using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallace.Domain.Entities;

namespace Wallace.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .Property(c => c.Name)
                .IsRequired();

            builder
                .Property(c => c.Emoji)
                .IsRequired();

            builder
                .HasMany(c => c.Transactions)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId);
            
            builder
                .HasOne(a => a.Owner)
                .WithMany(u => u.Categories)
                .HasForeignKey(a => a.OwnerId);
        }
    }
}