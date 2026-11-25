using ActionIn.Authentication.Data.Mappings;
using ActionIn.Authentication.Domain;
using ActionIn.Authentication.Domain.Errors;
using ActionIn.Core.Data;
using ActionIn.Core.Infrastructure.Interceptor;
using ActionIn.Core.Results;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ActionIn.Authentication.Data;

public class AuthenticationContext(DbContextOptions<AuthenticationContext> options, PublishDomainEventInterceptor interceptor) : DbContext(options), IUnitOfWork
{
    private const string UniqueViolation = "23505";

    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthenticationContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.AddInterceptors(interceptor);

    public async Task<Result> Commit(CancellationToken cancellationToken)
    {
        try
        {
            await base.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException
        {
            SqlState: UniqueViolation,
            ConstraintName: AccountMapping.EmailIndex or AccountMapping.UsernameIndex
        } postgresException)
        {
            return postgresException.ConstraintName == AccountMapping.EmailIndex
                ? Result.Failure(AuthenticationErrors.Email.AlreadyInUse)
                : Result.Failure(AuthenticationErrors.Username.AlreadyInUse);
        }
    }
}
