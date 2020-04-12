using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallace.Domain.Entities;

namespace Wallace.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(u => u.Email)
                .IsRequired();

            builder
                .Property(u => u.Name)
                .IsRequired();

            builder
                .Property(u => u.Password)
                .IsRequired();

            builder
                .HasIndex(u => u.Email);
        }
    }
}
