using ActionIn.Authentication.Application.Dtos;
using ActionIn.Authentication.Domain;
using ActionIn.Core.Data;

namespace ActionIn.Authentication.Data.Repository;

public class AccountRepository(AuthenticationContext context) : IAccountRepository
{
    public IUnitOfWork UnitOfWork => context;

    public void Register(Account account)
    {
        context.Accounts.Add(account);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
