using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ActionIn.Authentication.Domain;
using ActionIn.Authentication.Domain.ValueObjects;

namespace ActionIn.Authentication.Data.Mappings;

public class AccountMapping : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);

        builder.OwnsOne(a => a.Username, b =>
        {
            b.Property(u => u.Text)
                .HasColumnName("username")
                .HasMaxLength(Username.MaxLength)
                .IsRequired();
            b.HasIndex(u => u.Text).IsUnique();
        });

        builder.OwnsOne(a => a.Email, b =>
        {
            b.Property(e => e.Address)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            b.HasIndex(u => u.Address).IsUnique();
        });

        builder.OwnsOne(a => a.Password)
            .Property(p => p.Hash)
            .HasColumnName("password")
            .HasMaxLength(Password.HashMaxLength)
            .IsRequired();
    }
}
