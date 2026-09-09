using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ActionIn.Authentication.Domain;

namespace ActionIn.Authentication.Data.Mappings;

public class AccountMapping : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);

        builder.OwnsOne(a => a.Username)
            .Property(u => u.Value)
            .HasColumnName("username")
            .HasMaxLength(255)
            .IsRequired();

        builder.OwnsOne(a => a.Email)
            .Property(e => e.Value)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired();

        builder.OwnsOne(a => a.Password)
            .Property(p => p.Value)
            .HasColumnName("password")
            .HasMaxLength(255)
            .IsRequired();
    }
}
