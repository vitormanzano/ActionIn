using ActionIn.Authentication.Application.Dtos;
using ActionIn.Authentication.Domain;
using ActionIn.Authentication.Domain.Hasher;
using ActionIn.Authentication.Domain.Repository;

namespace ActionIn.Authentication.Application.Services;

public class AccountService(IAccountRepository accountRepository,
                            IPasswordHasher passwordHasher) : IAccountService
{
    public async Task<bool> RegisterAsync(RegisterAccountDto account)
    {
        var emailAlreadyExists = await accountRepository.GetByEmailAsync(account.Email);  
        if (emailAlreadyExists != null)
            throw new Exception("Email already exists");
        
        var usernameAlreadyExists = await accountRepository.GetByUsernameAsync(account.Username);
        if (usernameAlreadyExists != null)
            throw new Exception("Username already exists");
        
        accountRepository.Register(Account.Register(account.Username, account.Email, account.Password, passwordHasher));

        var success = await accountRepository.UnitOfWork.Commit();
        return !success ? throw new Exception("Failed to create account") : true;
    }

    public void Dispose()
    {
        accountRepository?.Dispose();
    }
}
