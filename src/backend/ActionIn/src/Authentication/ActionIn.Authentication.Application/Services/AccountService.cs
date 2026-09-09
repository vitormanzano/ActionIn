using ActionIn.Authentication.Application.Dtos;
using ActionIn.Authentication.Data.Repository;
using ActionIn.Authentication.Domain;
using ActionIn.Authentication.Domain.ValueObjects;
using ActionIn.Authentication.Domain.Hasher;

namespace ActionIn.Authentication.Application.Services;

public class AccountService(IAccountRepository accountRepository,
                            IPasswordHasher passwordHasher) : IAccountService
{
    public async Task<bool> RegisterAsync(RegisterAccountDto account)
    {
        var password = Password.Create(account.Password, passwordHasher);
        accountRepository.Register(Account.Register(account.Username, account.Email, password));

        var success = await accountRepository.UnitOfWork.Commit();
        return !success ? throw new Exception("Failed to create account") : true;
    }

    public void Dispose()
    {
        accountRepository?.Dispose();
    }
}
