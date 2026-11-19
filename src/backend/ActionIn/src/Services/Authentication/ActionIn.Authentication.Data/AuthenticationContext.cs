using ActionIn.Authentication.Domain;
using ActionIn.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace ActionIn.Authentication.Data;

public class AuthenticationContext(DbContextOptions<AuthenticationContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthenticationContext).Assembly);
    }
    public async Task Commit(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
    }
}
