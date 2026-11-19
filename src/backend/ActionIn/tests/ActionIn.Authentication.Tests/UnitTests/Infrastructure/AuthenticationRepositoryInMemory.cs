using ActionIn.Authentication.Domain;
using ActionIn.Authentication.Domain.Repository;
using ActionIn.Authentication.Domain.ValueObjects;
using ActionIn.Core.Data;

namespace ActionIn.Authentication.Tests.UnitTests.Infrastructure;

public class AuthenticationRepositoryInMemory : IAuthenticationRepository
{
    private readonly List<Account> _accounts = [];
    public IReadOnlyList<Account> Accounts => _accounts.AsReadOnly();

    public IUnitOfWork UnitOfWork => new FakeUnitOfWork();
    public void Register(Account account)
    {
        _accounts.Add(account);
    }

    public async Task<Account?> GetByUsernameAsync(string username)
    {
        return Accounts
            .FirstOrDefault(a => a.Username.Value == username);
    }
    public async Task<Account?> GetByEmailAsync(string email)
    {
        var normalized = Email.Create(email).Value.Value;

        return Accounts
            .FirstOrDefault(a => a.Email.Value == normalized);
    }
}


