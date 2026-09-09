using ActionIn.Authentication.Application.Dtos;
using ActionIn.Authentication.Domain;
using ActionIn.Authentication.Domain.ValueObjects;
using ActionIn.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace ActionIn.Authentication.Data.Repository;

public class AccountRepository(AuthenticationContext context) : IAccountRepository
{
    public IUnitOfWork UnitOfWork => context;

    public void Register(Account account)
    {
        context.Accounts.Add(account);
    }

    public async Task<Account?> GetByUsernameAsync(string username)
    {
        return await context.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Username.Value == username);
    }

    public async Task<Account?> GetByEmailAsync(string email)
    {
        var normalized = new Email(email).Value;
        
        return await context.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Email.Value == normalized);
        
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
