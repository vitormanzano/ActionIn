using ActionIn.Authentication.Domain;
using ActionIn.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace ActionIn.Authentication.Data;

public class AuthenticationContext(DbContextOptions<AuthenticationContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                    e => e.GetProperties())
                    .Where(p => p.ClrType == typeof(string)))
        {
            property.SetColumnType("varchar(100)");
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthenticationContext).Assembly);
    }
    public async Task<bool> Commit()
    {
        return await base.SaveChangesAsync() > 0;
    }
}
