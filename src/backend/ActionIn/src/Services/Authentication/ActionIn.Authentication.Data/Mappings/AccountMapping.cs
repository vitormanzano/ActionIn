using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ActionIn.Authentication.Domain;
using ActionIn.Authentication.Domain.ValueObjects;

namespace ActionIn.Authentication.Data.Mappings;

public class AccountMapping : IEntityTypeConfiguration<Account>
{
    public const string EmailIndex = "IX_Accounts_email";
    public const string UsernameIndex = "IX_Accounts_username";

    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);

        builder.OwnsOne(a => a.Username, b =>
        {
            b.Property(u => u.Text)
                .HasColumnName("username")
                .HasMaxLength(Username.MaxLength)
                .IsRequired();
            b.HasIndex(u => u.Text)
                .IsUnique()
                .HasDatabaseName(UsernameIndex);
        });

        builder.OwnsOne(a => a.Email, b =>
        {
            b.Property(e => e.Address)
                .HasColumnName("email")
                .HasMaxLength(Email.MaxLength)
                .IsRequired();

            b.HasIndex(u => u.Address)
                .IsUnique()
                .HasDatabaseName(EmailIndex);
        });

        builder.OwnsOne(a => a.Password)
            .Property(p => p.Hash)
            .HasColumnName("password")
            .HasMaxLength(Password.MaxLength)
            .IsRequired();

        builder.Ignore(u => u.DomainEvents);
    }
}
