using ActionIn.Authentication.Application.Dtos;
using ActionIn.Authentication.Domain;
using ActionIn.Authentication.Domain.Events.NewAccountRegistered;
using ActionIn.Authentication.Domain.Hasher;
using ActionIn.Authentication.Domain.Repository;
using ActionIn.Core.Mediatr;

namespace ActionIn.Authentication.Application.Services;

public class AccountService(IAccountRepository accountRepository,
                            IPasswordHasher passwordHasher,
                             IMediatrHandler _bus) : IAccountService
{
    public async Task<bool> RegisterAsync(RegisterAccountDto account)
    {
        var emailAlreadyExists = await accountRepository.GetByEmailAsync(account.Email);
        if (emailAlreadyExists is not null)
            throw new Exception("Email already exists");

        var usernameAlreadyExists = await accountRepository.GetByUsernameAsync(account.Username);
        if (usernameAlreadyExists is not null)
            throw new Exception("Username already exists");

        var entity = Account.Register(account.Username, account.Email, account.Password, passwordHasher);
        accountRepository.Register(entity);

        var success = await accountRepository.UnitOfWork.Commit();
        if (!success)
            throw new Exception("Something went wrong");

        await _bus.PublishEvent(new NewAccountRegisteredEvent(entity.Id, entity.Email.Value, entity.Username.Value));
        return true;
    }

    public async Task<bool> LoginAsync(LoginAccountDto account)
    {
        var existingAccount = await accountRepository.GetByEmailAsync(account.Email);

        return existingAccount is not null && existingAccount.VerifyPassword(account.Password, passwordHasher);
    }

    public void Dispose()
    {
        accountRepository?.Dispose();
    }
}
