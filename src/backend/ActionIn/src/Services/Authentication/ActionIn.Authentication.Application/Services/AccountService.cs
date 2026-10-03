using ActionIn.Authentication.Application.Dtos;
using ActionIn.Authentication.Domain;
using ActionIn.Authentication.Domain.Events.NewAccountRegistered;
using ActionIn.Authentication.Domain.Hasher;
using ActionIn.Authentication.Domain.Repository;
using ActionIn.Authentication.Domain.ValueObjects;
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

        Password.Validate(account.Password);

        var password = Password.FromHash(passwordHasher.Hash(account.Password));

        var entity = Account.Register(account.Username, account.Email, password);
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

        if (existingAccount is null)
            throw new Exception("Wrong credentials!");

        var isPasswordValid = passwordHasher.Verify(existingAccount.Password.Value, account.Password);

        if (!isPasswordValid)
            throw new Exception("Wrong credentials");

        return true;
    }

    public void Dispose()
    {
        accountRepository?.Dispose();
    }
}
